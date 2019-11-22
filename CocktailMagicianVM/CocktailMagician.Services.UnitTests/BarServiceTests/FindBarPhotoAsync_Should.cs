using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.BarServiceTests
{
    [TestClass]
   public class FindBarPhotoAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectBarPhotoFromGivenId()
        {
            //arrange
            string testBarName1 = "TestName1";
            int barId = 1;
            byte[] coverPhoto = new byte[0];
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnCorrectBarPhotoFromGivenId));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Bars.Add(new Bar() { Name = testBarName1,Id= barId }); ;
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.BarPhotos.Add(new BarPhoto() { BarCover = coverPhoto,Bar=actContext.Bars.First() }); 
                actContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                var barPhoto = await sut.FindBarPhotoAsync(barId);
                Assert.AreEqual(assertContext.Bars.First().Photo.BarCover, barPhoto.BarCover);
            }
        }
    }
}
