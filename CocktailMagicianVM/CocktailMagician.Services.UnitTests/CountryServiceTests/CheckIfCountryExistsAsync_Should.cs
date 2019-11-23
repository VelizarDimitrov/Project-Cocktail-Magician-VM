using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.CountryServiceTests
{
    [TestClass]
   public class CheckIfCountryExistsAsync_Should
    {
        [TestMethod]
        public async Task Return_True_ifContryExist()
        {
            string countryName = "countryTest";
            var options = TestUtilities.GetOptions(nameof(Return_True_ifContryExist));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Countries.Add(new Country() { Name = countryName });
                arrangeContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                var result = await sut.CheckIfCountryExistsAsync(countryName);
                Assert.AreEqual(true, result);
            }
        }
        [TestMethod]
        public async Task Return_False_ifContryDoesntExist()
        {
            string countryName = "countryTest";
            var options = TestUtilities.GetOptions(nameof(Return_False_ifContryDoesntExist));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                var result = await sut.CheckIfCountryExistsAsync(countryName);
                Assert.AreEqual(false, result);
            }
        }
    }
}
