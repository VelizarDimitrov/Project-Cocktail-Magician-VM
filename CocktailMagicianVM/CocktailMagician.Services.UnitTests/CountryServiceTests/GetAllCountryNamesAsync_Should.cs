using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.CountryServiceTests
{
    [TestClass]
   public class GetAllCountryNamesAsync_Should
    {
        [TestMethod]
        public async Task Correctly_ReturnAllCountryNames()
        {
            string countryName = "countryTest";
            string countryName2 = "countryNameTest2";
            var options = TestUtilities.GetOptions(nameof(Correctly_ReturnAllCountryNames));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Countries.Add(new Country() { Name = countryName });
                arrangeContext.Countries.Add(new Country() { Name = countryName2 });
                arrangeContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
             var countries = await sut.GetAllCountryNamesAsync();
                Assert.AreEqual(2, countries.Count());
            }
        }
    }
}
