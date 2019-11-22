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
    public class RateBarAsync_Should
    {
        [TestMethod]
        public async Task UpdatesExistingBR()
        {
            var testUserId = 1;
            var testBarId = 1;
            var testRating = 3;
            var testRatingNew = 5;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdatesExistingBR));

            using(var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.BarRating.Add(new BarRating() { BarId = testBarId, UserId = testUserId, Rating = testRating });
                arrangeContext.SaveChanges();
            }

            using(var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.RateBarAsync(testUserId, testRatingNew, testBarId);
            }

            using(var assertContext = new CocktailDatabaseContext(options))
            {
                var rating = assertContext.BarRating.First();
                Assert.AreEqual(testRatingNew, rating.Rating);
            }
        }

        [TestMethod]
        public async Task CreateNewBR()
        {

            var testUserId = 1;
            var testBarId = 1;
            var testRatingNew = 5;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>();
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(CreateNewBR));

            mockBarService.Setup(p => p.FindBarByIdAsync(testBarId))
                .Returns(Task.FromResult(new Bar { Id=testBarId,Name="se taq" }));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id=testUserId,UserName="se taq" });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService.Object, mockCocktailService);
                await sut.RateBarAsync(testUserId, testRatingNew, testBarId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var rating = assertContext.BarRating.First();
                Assert.AreEqual(testRatingNew, rating.Rating);
            }
        }
    }
}
