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
    public class FreezeUserAsync_Should
    {
        [TestMethod]
        public async Task FreezesUser()
        {
            var userId = 1;
            var status = "Active";
            var expected = "Frozen";

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(FreezesUser));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = userId, AccountStatus = status });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.FreezeUserAsync(userId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.First();
                Assert.AreEqual(expected, user.AccountStatus);
            }
        }
    }
}
