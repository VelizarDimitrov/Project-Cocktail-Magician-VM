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
    public class CreateIngredientAsync_Should
    {
        [TestMethod]
        public async Task CreatesSecondaryIngredient()
        {
            string ingredientName = "Ingredient";
            byte type = 0;

            var options = TestUtilities.GetOptions(nameof(CreatesSecondaryIngredient));

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(actContext);
                await sut.CreateIngredientAsync(ingredientName,type);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var ingredient = assertContext.Ingredients.First();
                Assert.IsNotNull(ingredient);
                Assert.AreEqual(type, ingredient.Primary);
                Assert.AreEqual(ingredientName.ToLower(), ingredient.Name);
            }
        }

        [TestMethod]
        public async Task CreatesPrimaryIngredient()
        {
            string ingredientName = "Ingredient";
            byte type = 1;

            var options = TestUtilities.GetOptions(nameof(CreatesPrimaryIngredient));

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(actContext);
                await sut.CreateIngredientAsync(ingredientName, type);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var ingredient = assertContext.Ingredients.First();
                Assert.IsNotNull(ingredient);
                Assert.AreEqual(type, ingredient.Primary);
                Assert.AreEqual(ingredientName.ToLower(), ingredient.Name);
            }
        }
    }
}
