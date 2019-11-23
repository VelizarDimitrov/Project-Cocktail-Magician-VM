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
    public class RemoveIngredientAsync_Should
    {
        [TestMethod]
        public async Task RemoveIngredient()
        {
            int ingId = 1;
            int ingId2 = 2;
            string ingredientName = "Ingredient";
            byte type = 0;

            var options = TestUtilities.GetOptions(nameof(RemoveIngredient));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = ingId, Name = ingredientName, Primary = type });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = ingId2, Name = ingredientName, Primary = type });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(actContext);
                await sut.RemoveIngredientAsync(ingId2);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var ingredient = assertContext.Ingredients.FirstOrDefault(p=>p.Id== ingId2);
                Assert.IsNull(ingredient);
            }
        }
    }
}
