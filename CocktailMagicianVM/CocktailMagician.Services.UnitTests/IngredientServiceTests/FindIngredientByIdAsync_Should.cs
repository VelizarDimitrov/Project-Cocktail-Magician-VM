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
    public class FindIngredientByIdAsync_Should
    {
        [TestMethod]
        public async Task ReturnsIngredientI()
        {
            int ingId = 1;
            string ingredientName = "Ingredient";
            byte type = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsIngredientI));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = ingId, Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ing = await sut.FindIngredientByIdAsync(ingId);
                Assert.IsNotNull(ing);
                Assert.AreEqual(type, ing.Primary);
                Assert.AreEqual(ingredientName.ToLower(), ing.Name.ToLower());
            }
        }

        [TestMethod]
        public async Task ReturnsNullI()
        {
            int ingId = 1;
            int wrongId = 2;
            string ingredientName = "Ingredient";
            byte type = 1;

            var options = TestUtilities.GetOptions(nameof(ReturnsNullI));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = ingId, Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ingredient = await sut.FindIngredientByIdAsync(wrongId);
                Assert.IsNull(ingredient);
            }
        }
    }
}
