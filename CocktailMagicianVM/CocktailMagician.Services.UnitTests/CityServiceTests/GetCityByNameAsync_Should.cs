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

namespace CocktailMagician.Services.UnitTests.CityServiceTests
{
    [TestClass]
   public class GetCityByNameAsync_Should
    {
        [TestMethod]
        public async Task Should_ReturnCity_FromGivenName()
        {
            //arrange
            string cityName1 = "testName1";
            var mockCountryService = new Mock<ICountryService>().Object;
            var options = TestUtilities.GetOptions(nameof(Should_ReturnCity_FromGivenName));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(arrangeContext, mockCountryService);
                arrangeContext.Cities.Add(new City() { Name = cityName1 });
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(actContext, mockCountryService);
                var city = await sut.GetCityByNameAsync(cityName1);
                Assert.IsNotNull(city);
                Assert.AreEqual(cityName1, city.Name);

            }
        }
    }
}
