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
   public class HideCocktailAsync_Should
    {
        [TestMethod]
        public async Task CorrectlyHideCocktail()
        {

            string cocktailName = "testName";
            int cocktailId = 14;
            double oldRating = 2.5;
            byte[] coverPhoto = new byte[0];
            int cocktailCommentsCount = 2;
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>().Object;
            var options = TestUtilities.GetOptions(nameof(CorrectlyHideCocktail));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {

                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId, Hidden = 0 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {

                var sut = new CocktailService(assertContext, mockIngredientService);
                await sut.HideCocktailAsync(cocktailId);
                Assert.AreEqual(1, assertContext.Cocktails.First().Hidden);
            }
        }
    }
}
