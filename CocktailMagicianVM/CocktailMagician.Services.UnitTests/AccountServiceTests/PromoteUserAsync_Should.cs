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
    public class PromoteUserAsync_Should
    {
        [TestMethod]
        public async Task PromotesUser()
        {
            var userId = 1;
            var type = "Bar Crawler";
            var expected = "Cocktail Magician";

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(PromotesUser));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = userId, AccountType= type });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.PromoteUserAsync(userId);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.First();
                Assert.AreEqual(expected, user.AccountType);
            }
        }
    }
}
