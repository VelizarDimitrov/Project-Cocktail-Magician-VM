using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.IngredientServiceTests
{
    [TestClass]
    public class GetAllIngredientNamesAsync_Should
    {
        [TestMethod]
        public async Task ReturnsIngredientNames()
        {
            byte primary = 1;
            byte secondary = 0;

            var options = TestUtilities.GetOptions(nameof(ReturnsIngredientNames));

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
                var ingredients = await sut.GetAllIngredientNamesAsync();
                Assert.AreEqual(5, ingredients.Count);
            }
        }
    }
}
