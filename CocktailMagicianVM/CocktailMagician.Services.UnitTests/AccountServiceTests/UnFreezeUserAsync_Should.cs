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
    public class UnFreezeUserAsync_Should
    {
        [TestMethod]
        public async Task UnfreezesUser()
        {
            var userId = 1;
            var status = "Frozen";
            var expected = "Active";

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UnfreezesUser));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id=userId, AccountStatus=status});
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.UnFreezeUserAsync(userId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.First();
                Assert.AreEqual(expected, user.AccountStatus);
            }
        }
    }
}
