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

namespace CocktailMagician.Services.UnitTests.CityServiceTests
{
    [TestClass]
   public class GetAllCityNamesAsync_Should
    {
        [TestMethod]
        public async Task Should_CorrectlyReturn_CityNames()
        {
            //arrange
            string cityName1 = "testName1";
            string cityName2 = "testName2";
            var mockCountryService = new Mock<ICountryService>().Object;          
            var options = TestUtilities.GetOptions(nameof(Should_CorrectlyReturn_CityNames));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(arrangeContext, mockCountryService);
                arrangeContext.Cities.Add(new City() { Name = cityName1});
                arrangeContext.Cities.Add(new City() { Name = cityName2});
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(actContext, mockCountryService);
                var cities = await sut.GetAllCityNamesAsync();
                Assert.AreEqual(cities.Count(), actContext.Cities.Count());
                Assert.AreEqual(cities[0], cityName1);
                Assert.AreEqual(cities[1], cityName2);

            }      
        }
    }
}
