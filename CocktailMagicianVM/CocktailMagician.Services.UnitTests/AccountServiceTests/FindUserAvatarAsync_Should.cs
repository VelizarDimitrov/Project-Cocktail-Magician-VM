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
    public class FindUserAvatarAsync_Should
    {
        [TestMethod]
        public async Task ReturnsNull_IncorrectId()
        {
            int testId1 = 1;
            int incorrectId1 = 2;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsNull_IncorrectId));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserPhotos.Add(new UserPhoto() {Id=testId1, UserId=testId1, UserCover = new byte[] { 1,4} });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                Assert.IsNull(await sut.FindUserAvatarAsync(incorrectId1));
            }
        }

        [TestMethod]
        public async Task ReturnsPhoto()
        {
            int testId1 = 1;
            var coverPhoto = new byte[]{ 1,4,5,6};
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsPhoto));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.UserPhotos.Add(new UserPhoto() { UserId = testId1, UserCover = coverPhoto });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var picture = await sut.FindUserAvatarAsync(testId1);
                Assert.IsInstanceOfType(picture.UserCover,typeof(byte[]));
            }
        }
    }
}
