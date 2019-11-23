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
   public class GetCountryByNameAsync_Should
    {
        [TestMethod]
        public async Task Return_Country_FromGivenName()
        {
            string countryName = "countryTest";
            var options = TestUtilities.GetOptions(nameof(Return_Country_FromGivenName));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Countries.Add(new Country() { Name = countryName });
                arrangeContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                var country = await sut.GetCountryByNameAsync(countryName);
                Assert.IsNotNull(country);
                Assert.AreEqual(countryName, country.Name);
            }
        }
    }
}
