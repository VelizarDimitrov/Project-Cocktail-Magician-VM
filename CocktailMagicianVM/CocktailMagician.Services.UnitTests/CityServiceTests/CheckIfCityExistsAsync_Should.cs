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
    public class CheckIfCityExistsAsync_Should
    {
        [TestMethod]
        public async Task Should_ReturnTrue_IfExist()
        {
            //arrange
            string cityName1 = "testName1";
            var mockCountryService = new Mock<ICountryService>().Object;
            var options = TestUtilities.GetOptions(nameof(Should_ReturnTrue_IfExist));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(arrangeContext, mockCountryService);
                arrangeContext.Cities.Add(new City() { Name = cityName1 });
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(actContext, mockCountryService);
                var checkIfExist = await sut.CheckIfCityExistsAsync(cityName1);
                Assert.AreEqual(true, checkIfExist);

            }
        }
        [TestMethod]
        public async Task Should_ReturnFalse_IfDoesntExist()
        {
            //arrange
            string cityName1 = "testName1";
            var mockCountryService = new Mock<ICountryService>().Object;
            var options = TestUtilities.GetOptions(nameof(Should_ReturnFalse_IfDoesntExist));
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CityService(actContext, mockCountryService);
                var checkIfExist = await sut.CheckIfCityExistsAsync(cityName1);
                Assert.AreEqual(false, checkIfExist);

            }
        }
    }
}
