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
    public class UpdateProfileAsync_Should
    {
        [TestMethod]
        public async Task UpdatesCreditentialsCorrectly()
        {
            var testId = 1;
            var testName1 = "TestName1";
            var testLastName1 = "TestLastName1";
            var testUsername1 = "TestUsername1";
            var testName2 = "TestName2";
            var testLastName2 = "TestLastName2";
            var testUsername2 = "TestUsername2";

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdatesCreditentialsCorrectly));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testId, UserName = testUsername1, FirstName = testName1, LastName = testLastName1 });
                arrangeContext.SaveChanges();
            }

            using(var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.UpdateProfileAsync(testId, testUsername2, testName2, testLastName2, null);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.First(p=>p.Id==testId);
                Assert.AreEqual(testName2, user.FirstName);
                Assert.AreEqual(testLastName2, user.LastName);
                Assert.AreEqual(testUsername2, user.UserName);
            }
        }

        [TestMethod]
        public async Task UpdatesPhotoCorrectly()
        {
            var testId = 1;
            byte[] testPhoto = new byte[1] { 1 };
            byte[] testPhoto2 = new byte[1] { 2 };

            var mockHasher = new Mock<IHashing>().Object;
            var mockBarService = new Mock<IBarService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var options = TestUtilities.GetOptions(nameof(UpdatesPhotoCorrectly));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = testId, UserName = "se taq", FirstName = "se taq", LastName = "se taq" });
                arrangeContext.SaveChanges();
                arrangeContext.UserPhotos.Add(new UserPhoto() { UserId = testId, UserCover = testPhoto });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new AccountService(actContext, mockHasher, mockBarService, mockCocktailService);
                await sut.UpdateProfileAsync(testId, "se taq", "se taq", "se taq", testPhoto2);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var photo = assertContext.UserPhotos.First(p => p.UserId == testId);
                Assert.AreEqual(testPhoto2[0], photo.UserCover[0]);
            }
        }
    }
}
