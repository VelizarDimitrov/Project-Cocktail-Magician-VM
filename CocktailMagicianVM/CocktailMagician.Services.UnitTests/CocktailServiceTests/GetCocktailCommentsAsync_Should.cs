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

namespace CocktailMagician.Services.UnitTests.CocktailServiceTests
{
    [TestClass]
   public class GetCocktailCommentsAsync_Should
    {
        [TestMethod]
        public async Task Shoud_ReturnCocktailCommentsCorrectly()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            int cocktailCommentsCount = 2;
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>().Object;

            var options = TestUtilities.GetOptions(nameof(Shoud_ReturnCocktailCommentsCorrectly));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId }); ;
                arrangeContext.SaveChanges();
            }
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User { UserName = "test" });
                arrangeContext.Users.Add(new User { UserName = "test2" });
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.CocktailComment.Add(new CocktailComment() { Cocktail = actContext.Cocktails.First(), User = actContext.Users.First() });
                actContext.CocktailComment.Add(new CocktailComment() { Cocktail = actContext.Cocktails.First(), User = actContext.Users.Skip(1).First() });
                actContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext,mockIngredientService);
                var comments = await sut.GetCocktailCommentsAsync(cocktailId,6);
                Assert.AreEqual(cocktailCommentsCount, comments.Count());
            }
        }
    }
}
