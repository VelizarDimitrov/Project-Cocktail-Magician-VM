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
   public class GetCitiesFromCountryAsync_Should
    {
        [TestMethod]
        public async Task Should_CorrectlyReturn_AllCityNamesFromGivenCountry()
        {
            //arrange
            string cityName1 = "testName1";
            string cityName2 = "testName2";
            string cityName3 = "testName3";
            string countryName = "countryTest";
            int countryId = 13;
            var mockCountryService = new Mock<ICountryService>();
            var options = TestUtilities.GetOptions(nameof(Should_CorrectlyReturn_AllCityNamesFromGivenCountry));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(arrangeContext, mockCountryService.Object);
                arrangeContext.Countries.Add(new Country() { Name = countryName, Id=countryId });
                arrangeContext.SaveChanges();
            }
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(arrangeContext, mockCountryService.Object);
                arrangeContext.Cities.Add(new City() { Name = cityName1,CountryId= countryId });
                arrangeContext.Cities.Add(new City() { Name = cityName2, CountryId = countryId });
                arrangeContext.Cities.Add(new City() { Name = cityName3 });
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(actContext, mockCountryService.Object);
                var cities = await sut.GetCitiesFromCountryAsync(countryName);
                Assert.AreEqual(2,cities.Count());

            }
        }
    }
}
