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
    public class FavoriteBarAsync_Should
    {
        [TestMethod]
        public async Task AddsToFavorite()
        {
            var userId = 1;
            var barId = 2;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>();
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(AddsToFavorite));

            mockBarService.Setup(p => p.FindBarByIdAsync(barId))
                .Returns(Task.FromResult(new Bar() { Id = barId }));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = userId });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService.Object, mockCocktailService);
                await sut.FavoriteBarAsync(userId, barId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var favoriteLink = assertContext.UserBar.FirstOrDefault(p => p.UserId == userId && p.BarId == barId);
                Assert.IsNotNull(favoriteLink);
            }

        }
    }
}
