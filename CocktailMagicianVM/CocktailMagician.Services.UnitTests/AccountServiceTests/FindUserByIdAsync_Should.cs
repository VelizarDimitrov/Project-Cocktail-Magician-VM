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
    public class FindUserByIdAsync_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "No user found.")]
        public async Task ThrowArgumentNullException_IncorrectId()
        {
            //arrange
            int testId1 = 1;
            int testId2 = 2;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentNullException_IncorrectId));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testId1 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var user = await sut.FindUserByIdAsync(testId2);
            }
        }

        [TestMethod]
        public async Task ReturnsUser()
        {
            //arrange
            int testId1 = 1;
            string testName = "TestName1";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsUser));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testId1, UserName = testName });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var user = await sut.FindUserByIdAsync(testId1);
                Assert.AreEqual(testName, user.UserName);
            }
        }
    }
}
