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
   public class UpdateAverageRatingAsync_Should
    {
        [TestMethod]

        public async Task UpdateCorrectlyCocktailAverageRating()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            int barId = 12;
            double oldRating = 2.5;
            byte[] coverPhoto = new byte[0];
            int cocktailCommentsCount = 2;
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>().Object;

            var options = TestUtilities.GetOptions(nameof(UpdateCorrectlyCocktailAverageRating));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId, AverageRating = oldRating }); ;
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
                actContext.CocktailRating.Add(new CocktailRating() { Cocktail = actContext.Cocktails.First(), User = actContext.Users.First(), Rating = 6 });
                actContext.CocktailRating.Add(new CocktailRating() { Cocktail = actContext.Cocktails.First(), User = actContext.Users.Skip(1).First(), Rating = 4 });
                actContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService);
                await sut.UpdateAverageRatingAsync(cocktailId);
                Assert.AreNotEqual(oldRating, assertContext.Cocktails.First().AverageRating);
            }
        }
    }
}
