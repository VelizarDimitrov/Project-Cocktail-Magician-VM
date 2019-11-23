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
    public class FindIngredientsByIdAsync_Should
    {
        [TestMethod]
        public async Task ReturnsIngredientsN()
        {
            string ingredientName = "Ingredient";
            byte type = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsIngredientsN));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = "se taq", Primary = type });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ing = await sut.FindIngredientsByNameAsync(ingredientName);
                Assert.AreEqual(2, ing.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsNoIngredientsN()
        {
            string ingredientName = "Ingredient";
            string wrongName = "se taq";
            byte type = 1;

            var options = TestUtilities.GetOptions(nameof(ReturnsNoIngredientsN));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ing = await sut.FindIngredientsByNameAsync(wrongName);
                Assert.AreEqual(0, ing.Count);
            }
        }
    }
}
