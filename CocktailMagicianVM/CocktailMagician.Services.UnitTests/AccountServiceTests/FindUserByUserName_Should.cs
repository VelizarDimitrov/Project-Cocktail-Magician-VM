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
    public class FindUserByUserName_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "No user found.")]
        public async Task ThrowArgumentNullException_NoUserFound()
        {
            //arrange
            string testUsername1 = "TestName1";
            string testUsername2 = "TestName2";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentNullException_NoUserFound));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { UserName = testUsername1 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var user = await sut.FindUserByUserNameAsync(testUsername2);
            }
        }

        [TestMethod]
        public async Task ReturnUser()
        {
            //arrange
            string testUsername = "TestName";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnUser));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { UserName = testUsername });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var user = await sut.FindUserByUserNameAsync(testUsername);
                Assert.AreEqual(testUsername, user.UserName);
            }
        }
    }
}
