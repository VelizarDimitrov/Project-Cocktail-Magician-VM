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
    public class RemoveCoctailBarAsync_Should
    {
        [TestMethod]

        public async Task ShouldRemoveCocktailBarCorrectly()
        {
            //arrange
            string barName = "testName";
            int barId = 13;
            int cocktailId = 6;
            int cityId = 7;
            int barCommentCount = 2;
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>();
            mockCocktailService.Setup(p => p.FindCocktailByIdAsync(cocktailId))
                .Returns(Task.FromResult(new Cocktail { Id = cocktailId, Name = "se taq" }));
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ShouldRemoveCocktailBarCorrectly));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cities.Add(new City() { Name = "TestCity", Id = cityId }); ;
                arrangeContext.SaveChanges();
            }
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Bars.Add(new Bar() { Name = barName, Id = barId, CityId = cityId }); ;
                arrangeContext.SaveChanges();
            }
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.BarCocktail.Add(new BarCocktail() { BarId = barId, CocktailId = cocktailId }); ;
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService.Object, mockNotificationService);
                await sut.RemoveCoctailBarAsync(barId, cocktailId);
                Assert.AreEqual(0, assertContext.BarCocktail.Count());
            }
        }

    }
}
