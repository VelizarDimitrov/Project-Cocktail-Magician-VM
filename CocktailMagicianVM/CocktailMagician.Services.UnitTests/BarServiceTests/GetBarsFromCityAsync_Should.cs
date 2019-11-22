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
    public class GetBarsFromCityAsync_Should
    {
        [TestMethod]
        public async Task ReturnAllBarNamesFromCityCorrectly()
        {
            //arrange
            string cityName = "testCity";
            string cityName2 = "testCity2";
            string testBarName1 = "TestName1";
            string testBarName2 = "TestName2";
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnAllBarNamesFromCityCorrectly));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cities.Add(new City() { Name = cityName, Id = 1 });
                arrangeContext.Cities.Add(new City() { Name = cityName2, Id = 2 });
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.Bars.Add(new Bar() { Name = testBarName1, CityId = 2 }); ;
                actContext.Bars.Add(new Bar() { Name = testBarName2, CityId = 2 });
                actContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                var bars = await sut.GetBarsFromCityAsync(cityName2);
                Assert.AreEqual(bars[0], testBarName1);
                Assert.AreEqual(bars[1], testBarName2);
            }
        }
    }
}
