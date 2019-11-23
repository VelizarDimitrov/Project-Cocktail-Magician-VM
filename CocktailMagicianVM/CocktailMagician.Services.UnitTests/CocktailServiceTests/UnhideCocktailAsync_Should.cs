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
   public class UnhideCocktailAsync_Should
    {
        [TestMethod]
        public async Task CorrectlyUnHideCocktail()
        {

            string cocktailName = "testName";
            int cocktailId = 14;
            double oldRating = 2.5;
            byte[] coverPhoto = new byte[0];
            int cocktailCommentsCount = 2;
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>().Object;
            var options = TestUtilities.GetOptions(nameof(CorrectlyUnHideCocktail));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {

                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId, Hidden = 1 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {

                var sut = new CocktailService(assertContext, mockIngredientService);
                await sut.UnhideCocktailAsync(cocktailId);
                Assert.AreEqual(0, assertContext.Cocktails.First().Hidden);
            }
        }
    }
}
