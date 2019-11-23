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
    public class FavoriteCocktailAsync_Should
    {
        [TestMethod]
        public async Task AddsFavoriteC()
        {
            var userId = 1;
            var cocktailId = 2;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>();
            var options = TestUtilities.GetOptions(nameof(AddsFavoriteC));

            mockCocktailService.Setup(p => p.FindCocktailByIdAsync(cocktailId))
                .Returns(Task.FromResult(new Cocktail() { Id = cocktailId }));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = userId });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService.Object);
                await sut.FavoriteCocktailAsync(userId, cocktailId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var favoriteLink = assertContext.UserCocktail.FirstOrDefault(p => p.UserId == userId && p.CocktailId == cocktailId);
                Assert.IsNotNull(favoriteLink);
            }

        }
    }
}
