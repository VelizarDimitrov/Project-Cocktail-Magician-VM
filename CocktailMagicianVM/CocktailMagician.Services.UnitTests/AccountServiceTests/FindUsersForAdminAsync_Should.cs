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
    public class FindUsersForAdminAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectAmount()
        {
            var users = new List<User>();
            var usersCount = 6;
            var keyword = "testName";
            var pageSize = 5;
            var page = 1;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnCorrectAmount));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = 1, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 2, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 3, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 4, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 5, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 6, UserName = keyword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var result = await sut.FindUsersForAdminAsync(keyword, page, pageSize);
                Assert.AreEqual(pageSize, result.Item1.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsLastPage()
        {
            var users = new List<User>();
            var usersCount = 6;
            var keyword = "testName";
            var pageSize = 5;
            var countExpected = usersCount % pageSize;
            var lastPage = usersCount / pageSize + 1;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsLastPage));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = 1, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 2, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 3, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 4, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 5, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 6, UserName = keyword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var result = await sut.FindUsersForAdminAsync(keyword, lastPage, pageSize);
                Assert.AreEqual(countExpected, result.Item1.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsCorrectUser()
        {
            var users = new List<User>();
            var keyword = "testName";
            var pageSize = 5;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsCorrectUser));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = 1, UserName = keyword });
                arrangeContext.Users.Add(new User() { Id = 2, UserName = "se Taq", City = "se Taq", Country = "se Taq" });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var result = await sut.FindUsersForAdminAsync(keyword, 1, pageSize);
                Assert.AreEqual(keyword, result.Item1[0].UserName);
                Assert.AreEqual(1, result.Item1.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyList()
        {
            var users = new List<User>();
            var keyword = "testName";
            var pageSize = 5;

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnsEmptyList));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = 1, UserName = "se Taq", City = "se Taq", Country = "se Taq" });
                arrangeContext.Users.Add(new User() { Id = 2, UserName = "se Taq", City = "se Taq", Country = "se Taq" });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(assertContext, mockHasher, mockBarService, mockCocktailService);
                var result = await sut.FindUsersForAdminAsync(keyword, 1, pageSize);
                Assert.IsTrue(!result.Item1.Any());
            }
        }
    }
}
