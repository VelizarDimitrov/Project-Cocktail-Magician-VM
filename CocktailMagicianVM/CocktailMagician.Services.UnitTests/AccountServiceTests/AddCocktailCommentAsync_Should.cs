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
    public class AddCocktailCommentAsync_Should
    {
        [TestMethod]
        public async Task UpdatesExistingCC()
        {
            var testUserId = 1;
            var testCocktailId = 1;
            var testComment = "Initial and plain comment.";
            var testCommentNew = "New and improved comment.";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdatesExistingCC));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.CocktailComment.Add(new CocktailComment() { CocktailId = testCocktailId, UserId = testUserId, Comment = testComment });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.AddCocktailCommentAsync(testUserId, testCommentNew, testCocktailId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var comment = assertContext.CocktailComment.First();
                Assert.AreEqual(testCommentNew, comment.Comment);
            }
        }

        [TestMethod]
        public async Task CreateNewCC()
        {
            var testUserId = 1;
            var testCocktailId = 1;
            var testComment = "Initial and plain comment.";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>();
            var options = TestUtilities.GetOptions(nameof(CreateNewCC));

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
                await sut.AddCocktailCommentAsync(testUserId, testComment, testCocktailId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var comment = assertContext.CocktailComment.First();
                Assert.AreEqual(testComment, comment.Comment);
            }
        }
    }
}
