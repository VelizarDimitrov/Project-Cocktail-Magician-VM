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
    public class VerifyUserPasswordAsync_Should
    {
        [TestMethod]
        public async Task ReturnsFalse_IncorrectPassword()
        {
            var testUsername = "TestName";
            var testPassword = "123456";
            var hashedPassword = "hashed!!!!";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsFalse_IncorrectPassword));

            mockHasher.Setup(m => m.Verify(testPassword, hashedPassword))
                .Returns(false);

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { UserName = testUsername, Password=hashedPassword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher.Object, mockBarService, mockCocktailService);
                var response = await sut.VerifyUserPasswordAsync(testUsername,testPassword);
                Assert.IsFalse(response);
            }
        }

        [TestMethod]
        public async Task ReturnsTrue_CorrectPassword()
        {
            var testUsername = "TestName";
            var testPassword = "123456";
            var hashedPassword = "hashed!!!!";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsFalse_IncorrectPassword));

            mockHasher.Setup(m => m.Verify(testPassword, hashedPassword))
                .Returns(true);

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { UserName = testUsername, Password = hashedPassword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher.Object, mockBarService, mockCocktailService);
                var response = await sut.VerifyUserPasswordAsync(testUsername, testPassword);
                Assert.IsTrue(response);
            }
        }
    }
}
