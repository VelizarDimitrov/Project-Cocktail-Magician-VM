using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.BarServiceTests
{
    [TestClass]
   public class BarsCountAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectBarCount()
        {
            //arrange
            int count = 2;
            string testBarName1= "TestName1";
            string testBarName2 = "TestName2";
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnCorrectBarCount));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Bars.Add(new Bar() { Name = testBarName1 });
                arrangeContext.Bars.Add(new Bar() { Name = testBarName2 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                var barsCount = await sut.BarsCountAsync();
                Assert.AreEqual(count, barsCount);
                

            }
        }
    }
}
