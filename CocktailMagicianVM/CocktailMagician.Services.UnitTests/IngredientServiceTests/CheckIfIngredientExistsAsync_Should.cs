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
    public class CheckIfIngredientExistsAsync_Should
    {
        [TestMethod]
        public async Task ReturnsTrue()
        {
            string ingredientName = "Ingredient";
            byte type = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsTrue));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var resul = await sut.CheckIfIngredientExistsAsync(ingredientName, type);
                Assert.IsTrue(resul);
            }
        }

        [TestMethod]
        public async Task ReturnsFalse()
        {
            string ingredientName = "Ingredient";
            string wrongName = "se taq";
            byte type = 1;

            var options = TestUtilities.GetOptions(nameof(ReturnsFalse));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var resu = await sut.CheckIfIngredientExistsAsync(wrongName, type);
                Assert.IsFalse(resu);
            }
        }
    }
}
