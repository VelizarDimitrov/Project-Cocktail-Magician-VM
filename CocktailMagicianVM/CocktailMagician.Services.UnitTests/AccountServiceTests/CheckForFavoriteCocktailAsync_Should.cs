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

namespace CocktailMagician.Services.UnitTests.AccountServiceTests
{
    [TestClass]
    public class CheckForFavoriteCocktailAsync_Should
    {
        [TestMethod]
        public async Task ReturnExist()
        {
            var userId = 1;
            var cocktailId = 1;
            var expected = "exist";

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnExist));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserCocktail.Add(new UserCocktail() { UserId = userId, CocktailId = cocktailId });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var result = await sut.CheckForFavoriteCocktailAsync(userId, cocktailId);
                Assert.AreEqual(expected, result);
            }
        }

        [TestMethod]
        public async Task ReturnNotExist()
        {
            var userId = 1;
            var cocktailId = 1;
            var fakeId = 2;
            var expected = "not exist";

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnNotExist));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserCocktail.Add(new UserCocktail() { UserId = userId, CocktailId = cocktailId });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var result = await sut.CheckForFavoriteCocktailAsync(fakeId, fakeId);
                Assert.AreEqual(expected, result);
            }
        }
    }
}
