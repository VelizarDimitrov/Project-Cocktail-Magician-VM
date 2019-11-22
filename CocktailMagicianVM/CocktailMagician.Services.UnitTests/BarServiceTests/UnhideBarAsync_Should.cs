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
   public class UnhideBarAsync_Should
    {
        [TestMethod]
        public async Task CorrectlyUnHideBar()
        {

            //arrange
            int testId = 12;
            string testBarName1 = "TestName1";
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(CorrectlyUnHideBar));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {

                arrangeContext.Bars.Add(new Bar() { Name = testBarName1, Id = testId, Hidden = 0 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {

                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.UnhideBarAsync(testId);
                Assert.AreEqual(0, assertContext.Bars.First().Hidden);
            }
        }
    }
}
