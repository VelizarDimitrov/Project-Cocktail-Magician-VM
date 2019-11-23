using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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
   public class CreateCountryAsync_Should
    {
        [TestMethod]
        public async Task Create_Country_Correctly()
        {
            //arrange

            string countryName = "countryTest";   
            var options = TestUtilities.GetOptions(nameof(Create_Country_Correctly));
       
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                await sut.CreateCountryAsync(countryName);;
                Assert.AreEqual(1, assertContext.Countries.Count());
                var city = await assertContext.Countries.FirstOrDefaultAsync(u => u.Name == countryName);
                Assert.IsNotNull(city);
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException),"Country name cannot be null or whitespace.")]
        public async Task Should_ThrowArgumentNullException_WhenNameIsNull()
        {
            string countryName = "countryTest";
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenNameIsNull));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                await sut.CreateCountryAsync(null);            
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Country name cannot be null or whitespace.")]
        public async Task Should_ThrowArgumentNullException_WhenNameIsWhitespace()
        {
            string countryName = "countryTest";
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenNameIsWhitespace));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                await sut.CreateCountryAsync("");
            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Country with that name already exists!")]
        public async Task Should_ThrowArgumentException_WhenNameExist()
        {
            string countryName = "countryTest";
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentException_WhenNameExist));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Countries.Add(new Country() { Name = countryName });
                arrangeContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CountryService(assertContext);
                await sut.CreateCountryAsync(countryName);
            }
        }
    }
}
