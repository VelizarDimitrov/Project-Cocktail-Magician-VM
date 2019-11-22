using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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
   public class CreateCityAsync_Should
    {
        [TestMethod]
        public async Task Create_City_Correctly()
        {
            //arrange
            string cityName = "testName";
            string countryName = "countryTest";
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(p => p.CheckIfCountryExistsAsync(countryName))
              .Returns(Task.FromResult(true));
            mockCountryService.Setup(p => p.GetCountryByNameAsync(countryName))
               .Returns(Task.FromResult(new Country() { Name = countryName}));
            var options = TestUtilities.GetOptions(nameof(Create_City_Correctly));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(assertContext, mockCountryService.Object);
                await sut.CreateCityAsync(cityName, countryName);
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                Assert.AreEqual(1, assertContext.Cities.Count());
                var city = await assertContext.Cities.FirstOrDefaultAsync(u => u.Name == cityName);
                Assert.IsNotNull(city);


            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "City name cannot be null or empty.")]
        public async Task Should_ThrowArgumentException_WhenNameIsNull()
        {
            //arrange
            string countryName = "countryTest";
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(p => p.CheckIfCountryExistsAsync(countryName))
              .Returns(Task.FromResult(true));
            mockCountryService.Setup(p => p.GetCountryByNameAsync(countryName))
               .Returns(Task.FromResult(new Country() { Name = countryName }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentException_WhenNameIsNull));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(assertContext, mockCountryService.Object);
                await sut.CreateCityAsync(null, countryName);
            }         
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "City name cannot be null or empty.")]
        public async Task Should_ThrowArgumentException_WhenNameIsWhiteSpace()
        {
            //arrange
            string countryName = "countryTest";
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(p => p.CheckIfCountryExistsAsync(countryName))
              .Returns(Task.FromResult(true));
            mockCountryService.Setup(p => p.GetCountryByNameAsync(countryName))
               .Returns(Task.FromResult(new Country() { Name = countryName }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentException_WhenNameIsWhiteSpace));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(assertContext, mockCountryService.Object);
                await sut.CreateCityAsync("", countryName);
            }
        }
    }
}
