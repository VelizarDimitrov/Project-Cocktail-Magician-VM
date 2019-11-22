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
    public class UpdatePasswordAsync_Should
    {
        [TestMethod]
        public async Task UpdateCorrectly()
        {
            var testId = 1;
            var testPassword = "123456";
            var oldHashed = "hashed!!!!";
            var newHashed = "hashed????";

            var mockHasher = new Mock<IHashing>();
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdateCorrectly));

            mockHasher.Setup(p => p.Hash(testPassword))
                .Returns(newHashed);

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testId, Password = oldHashed });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher.Object, mockBarService, mockCocktailService);
                await sut.UpdatePasswordAsync(testId, testPassword);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.First(p => p.Id == testId);
                Assert.AreEqual(newHashed, user.Password);
            }

        }
    }
}
