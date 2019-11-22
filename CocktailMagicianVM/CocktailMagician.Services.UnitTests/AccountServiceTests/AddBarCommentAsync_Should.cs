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
    public class AddBarCommentAsync_Should
    {
        [TestMethod]
        public async Task UpdatesExistingBC()
        {
            var testUserId = 1;
            var testBarId = 1;
            var testComment = "Initial and plain comment.";
            var testCommentNew = "New and improved comment.";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdatesExistingBC));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.BarComment.Add(new BarComment() { BarId = testBarId, UserId = testUserId, Comment = testComment });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.AddBarCommentAsync(testUserId, testCommentNew, testBarId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var comment = assertContext.BarComment.First();
                Assert.AreEqual(testCommentNew, comment.Comment);
            }
        }

        [TestMethod]
        public async Task CreateNewBC()
        {
            var testUserId = 1;
            var testBarId = 1;
            var testComment = "Initial and plain comment.";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>();
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(CreateNewBC));

            mockBarService.Setup(p => p.FindBarByIdAsync(testBarId))
                .Returns(Task.FromResult(new Bar { Id = testBarId, Name = "se taq" }));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testUserId, UserName = "se taq" });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService.Object, mockCocktailService);
                await sut.AddBarCommentAsync(testUserId, testComment, testBarId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var comment = assertContext.BarComment.First();
                Assert.AreEqual(testComment, comment.Comment);
            }
        }
    }
}
