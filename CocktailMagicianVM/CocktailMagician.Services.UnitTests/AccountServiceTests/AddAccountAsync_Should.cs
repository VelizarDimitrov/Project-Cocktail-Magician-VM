using Data;
using Microsoft.EntityFrameworkCore;
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
    //CocktailDatabaseContext dbContext, IHashing hasher, IBarService barService, ICocktailService cService
    [TestClass]
    public class AddAccountAsync_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException),"Username cannot be null or empty.")]
        public async Task ThrowArgumentException_NullUsername()
        {
            //arrange
            string testUsername = null;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_NullUsername));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext,mockHasher,mockBarService,mockCocktailService);
                await sut.AddAccountAsync(testUsername, "se taq", "se taq", "se taq", "se taq", "se taq", "se taq");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username cannot be null or empty.")]
        public async Task ThrowArgumentException_EmptyUsername()
        {
            //arrange
            string testUsername = "";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_NullUsername));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                await sut.AddAccountAsync(testUsername, "se taq", "se taq", "se taq", "se taq", "se taq", "se taq");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password is required and must be at least 6 characters long.")]
        public async Task ThrowArgumentException_NullPassword()
        {
            //arrange
            string testPassword = null;
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_NullPassword));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                await sut.AddAccountAsync("se taq", "se taq", "se taq", testPassword, "se taq", "se taq", "se taq");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Password is required and must be at least 6 characters long.")]
        public async Task ThrowArgumentException_ShortPassword()
        {
            //arrange
            string testPassword = "12345";
            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_ShortPassword));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                await sut.AddAccountAsync("se taq", "se taq", "se taq", testPassword, "se taq", "se taq", "se taq");
            }

        }

        [TestMethod]
        public async Task Create_User()
        {
            var testUsername = "TestName";
            var testPassword = "123456";
            var hashedPassword = "hashed!!!!";


            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(Create_User));

            mockHasher.Setup(m => m.Hash(testPassword))
                .Returns(hashedPassword);

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher.Object, mockBarService, mockCocktailService);
                await sut.AddAccountAsync(testUsername, "se taq", "se taq", testPassword, "se taq", "se taq", "se taq");
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                Assert.AreEqual(1, assertContext.Users.Count());
                var user = await assertContext.Users.FirstOrDefaultAsync(u => u.UserName == testUsername);
                Assert.IsNotNull(user);
                Assert.AreEqual(hashedPassword, user.Password);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username is already taken.")]
        public async Task ThrowArgumentException_UserExists()
        {
            var testUsername = "TestName";
            var testPassword = "123456";
            var hashedPassword = "hashed!!!!";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_UserExists));

            mockHasher.Setup(m => m.Hash(testPassword))
                .Returns(hashedPassword);

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher.Object, mockBarService, mockCocktailService);
                await sut.AddAccountAsync(testUsername, "se taq", "se taq", testPassword, "se taq", "se taq", "se taq");
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher.Object, mockBarService, mockCocktailService);
                await sut.AddAccountAsync(testUsername, "se taq", "se taq", testPassword, "se taq", "se taq", "se taq");
            }


        }
    }
}
