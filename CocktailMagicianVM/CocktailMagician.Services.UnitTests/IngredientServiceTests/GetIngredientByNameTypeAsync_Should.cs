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
    public class GetIngredientByNameTypeAsync_Should
    {
        [TestMethod]
        public async Task ReturnsIngredientN()
        {
            string ingredientName = "Ingredient";
            byte type = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsIngredientN));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ing = await sut.GetIngredientByNameTypeAsync(ingredientName, type);
                Assert.IsNotNull(ing);
                Assert.AreEqual(type, ing.Primary);
                Assert.AreEqual(ingredientName.ToLower(), ing.Name.ToLower());
            }
        }

        [TestMethod]
        public async Task ReturnsNull()
        {
            string ingredientName = "Ingredient";
            byte type = 1;

            var options = TestUtilities.GetOptions(nameof(ReturnsNull));


            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var ingredient = await sut.GetIngredientByNameTypeAsync(ingredientName, type);
                Assert.IsNull(ingredient);
            }
        }
    }
}
