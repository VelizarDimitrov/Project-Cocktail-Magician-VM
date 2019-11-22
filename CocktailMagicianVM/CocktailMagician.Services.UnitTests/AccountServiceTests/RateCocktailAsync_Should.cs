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
    public class RateCocktailAsync_Should
    {
        [TestMethod]
        public async Task UpdatesExistingCR()
        {
            var testUserId = 1;
            var testCocktailId = 1;
            var testRating = 3;
            var testRatingNew = 5;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdatesExistingCR));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.CocktailRating.Add(new CocktailRating() { CocktailId = testCocktailId, UserId = testUserId, Rating = testRating });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.RateCocktailAsync(testUserId, testRatingNew, testCocktailId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var rating = assertContext.CocktailRating.First();
                Assert.AreEqual(testRatingNew, rating.Rating);
            }
        }

        [TestMethod]
        public async Task CreateNewCR()
        {
            var testUserId = 1;
            var testCocktailId = 1;
            var testRatingNew = 5;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>();
            var options = TestUtilities.GetOptions(nameof(CreateNewCR));

            mockCocktailService.Setup(p => p.FindCocktailByIdAsync(testCocktailId))
                .Returns(Task.FromResult(new Cocktail { Id = testCocktailId, Name = "se taq" }));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testUserId, UserName = "se taq" });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService.Object);
                await sut.RateCocktailAsync(testUserId, testRatingNew, testCocktailId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var rating = assertContext.CocktailRating.First();
                Assert.AreEqual(testRatingNew, rating.Rating);
            }
        }
    }
}
