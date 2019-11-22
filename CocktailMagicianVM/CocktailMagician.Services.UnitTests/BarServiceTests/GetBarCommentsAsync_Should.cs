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
   public class GetBarCommentsAsync_Should
    {
        [TestMethod]

        public async Task Shoud_ReturnCommentsCorrectly()
        {
            //arrange
            string barName = "testName";
            int barId = 12;
            double oldRating = 2.5;
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(Shoud_ReturnCommentsCorrectly));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Bars.Add(new Bar() { Name = barName, AverageRating = oldRating, Id = barId }); ;
                arrangeContext.SaveChanges();
            }
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User { UserName = "test" });
                arrangeContext.Users.Add(new User { UserName = "test2" });
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.BarRating.Add(new BarRating() { Bar = actContext.Bars.First(), User = actContext.Users.First(), Rating = 6 });
                actContext.BarRating.Add(new BarRating() { Bar = actContext.Bars.First(), User = actContext.Users.Skip(1).First(), Rating = 4 });
                actContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.UpdateAverageRatingAsync(barId);
                Assert.AreNotEqual(oldRating, assertContext.Bars.First().AverageRating);
            }
        }
    }
}
