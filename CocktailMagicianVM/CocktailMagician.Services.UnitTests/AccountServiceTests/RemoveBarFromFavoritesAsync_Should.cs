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

namespace CocktailMagician.Services.UnitTests.AccountServiceTests
{
    [TestClass]
    public class RemoveBarFromFavoritesAsync_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ThrowArgumentException_WhenNotFound()
        {
            var userId = 1;
            var barId = 2;
            var incorrectId = 3;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_WhenNotFound));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserBar.Add(new UserBar() { BarId = barId, UserId = userId });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                await sut.RemoveBarFromFavoritesAsync(incorrectId, incorrectId);
            }

        }

        [TestMethod]
        public async Task RemovesFavorite()
        {
            var userId = 1;
            var barId = 2;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(RemovesFavorite));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserBar.Add(new UserBar() { BarId = barId, UserId = userId });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.RemoveBarFromFavoritesAsync(barId, userId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var fav = assertContext.UserBar.FirstOrDefault(p => p.BarId == barId && p.UserId == userId);
                Assert.IsNull(fav);
            }
        }

    }
}
