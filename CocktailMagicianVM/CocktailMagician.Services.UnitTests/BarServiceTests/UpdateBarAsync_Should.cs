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
    public class UpdateBarAsync_Should
    {
        [TestMethod]

        public async Task Should_UpdateBarCorrectly()
        {
            //arrange
            string barName = "testName";
            string barNewName = "testNewName";
            int barId = 13;
            int cocktailId = 6;
            int cityId = 7;
            string cityName = "CityName";
            int countryId = 7;
            string countryName = "CountryName";
            string address = "testAddress";
            string description = "testdescription";
            var mockCountryService = new Mock<ICountryService>();
            mockCountryService.Setup(p => p.CheckIfCountryExistsAsync(countryName))
                .Returns(Task.FromResult(true));
            mockCountryService.Setup(p => p.GetCountryByNameAsync(countryName))
               .Returns(Task.FromResult(new Country() { Name = countryName, Id = countryId }));
            var mockCityService = new Mock<ICityService>();
            mockCityService.Setup(p => p.CheckIfCityExistsAsync(cityName))
               .Returns(Task.FromResult(true));
            mockCityService.Setup(p => p.GetCityByNameAsync(cityName))
              .Returns(Task.FromResult(new City() { Name = cityName, Id = cityId }));
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(Should_UpdateBarCorrectly));       
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Bars.Add(new Bar() { Name = barName, Id = barId}); ;
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService.Object, mockCityService.Object, mockCocktailService, mockNotificationService);
                await sut.UpdateBarAsync(barId, barNewName, address, description, countryName, cityName,null);
                Assert.AreEqual(barNewName, assertContext.Bars.First().Name);
                Assert.AreEqual(address, assertContext.Bars.First().Address);
                Assert.AreEqual(description, assertContext.Bars.First().Description);
                Assert.AreEqual(cityId, assertContext.Bars.First().CityId);
                Assert.AreEqual(countryId, assertContext.Bars.First().CountryId);
            }
        }
    }
}
