using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.IngredientServiceTests
{
    [TestClass]
    public class GetAllMainIngredientsAsync_Should
    {
        [TestMethod]
        public async Task ReturnsMainIngredients()
        {
            byte primary = 1;
            byte secondary = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsMainIngredients));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = primary });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = primary });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = primary });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = secondary });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = secondary });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ingredients = await sut.GetAllMainIngredientsAsync();
                Assert.AreEqual(3, ingredients.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsNoIngredients()
        {
            byte secondary = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsNoIngredients));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = secondary });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "Se Taq", Primary = secondary });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ingredients = await sut.GetAllMainIngredientsAsync();
                Assert.AreEqual(0, ingredients.Count);
            }
        }
    }
}
