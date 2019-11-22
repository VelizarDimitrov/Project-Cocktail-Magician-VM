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
    public class FindUserWebAsync_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ThrowsArgumentException_UsernameIncorrect()
        {
            var testUsername = "TestName";
            var incorrectUsername = "TestName2";
            var testPassword = "123456";
            var hashedPassword = "hashed!!!!";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowsArgumentException_UsernameIncorrect));

            mockHasher.Setup(m => m.Verify(testPassword, hashedPassword))
                .Returns(true);

            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.Users.Add(new User { UserName = testUsername, Password = hashedPassword });
                actContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher.Object, mockBarService, mockCocktailService);
                await sut.FindUserWebAsync(incorrectUsername, testPassword);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public async Task ThrowsArgumentException_PasswordIncorrect()
        {
            var testUsername = "TestName";
            var testPassword = "123456";
            var incorrectPassword = "654321";
            var hashedPassword = "hashed!!!!";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowsArgumentException_PasswordIncorrect));

            mockHasher.Setup(m => m.Verify(testPassword, hashedPassword))
                .Returns(true);

            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.Users.Add(new User { UserName = testUsername, Password = hashedPassword });
                actContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher.Object, mockBarService, mockCocktailService);
                await sut.FindUserWebAsync(testUsername, incorrectPassword);
            }
        }

        [TestMethod]
        public async Task ReturnsUser()
        {
            var testUsername = "TestName";
            var testPassword = "123456";
            var hashedPassword = "hashed!!!!";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsUser));

            mockHasher.Setup(m => m.Verify(testPassword, hashedPassword))
                .Returns(true);

            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.Users.Add(new User { UserName = testUsername, Password = hashedPassword });
                actContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher.Object, mockBarService, mockCocktailService);
                var user = await sut.FindUserWebAsync(testUsername, testPassword);
                Assert.AreEqual(testUsername, user.UserName);
            }
        }
    }
}
