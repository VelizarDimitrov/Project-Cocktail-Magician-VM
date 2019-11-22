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
    public class RemoveCocktailFromFavoritesAsync_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task ThrowArgumentException_WhenNotFound()
        {
            var userId = 1;
            var cocktailId = 2;
            var incorrectId = 3;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_WhenNotFound));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserCocktail.Add(new UserCocktail() { CocktailId = cocktailId, UserId = userId });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                await sut.RemoveCocktailFromFavoritesAsync(incorrectId, incorrectId);
            }

        }

        [TestMethod]
        public async Task RemovesFavorite()
        {
            var userId = 1;
            var cocktailId = 2;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(RemovesFavorite));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserCocktail.Add(new UserCocktail() { CocktailId = cocktailId, UserId = userId });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.RemoveCocktailFromFavoritesAsync(cocktailId, userId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var fav = assertContext.UserCocktail.FirstOrDefault(p => p.CocktailId == cocktailId && p.UserId == userId);
                Assert.IsNull(fav);
            }
        }

    }
}
