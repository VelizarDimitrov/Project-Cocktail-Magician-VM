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
   public class UpdateCocktailAsync_Should
    {
        [TestMethod]

        public async Task Should_UpdateCocktailCorrectly()
        {
            //arrange
            string cocktailName = "testName";
            string cocktailNewName = "testNewName";
            int cocktailId = 14;
            string description = "testDescription";
            byte[] coverPhoto = new byte[0];
            int cocktailCommentsCount = 2;
            int ingredientId = 21;
            int ingredientId2 = 22;
            IList<CocktailIngredient> testList = new List<CocktailIngredient>();
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 1))
            .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 0))
           .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1,Id =ingredientId }));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 0))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 0, Id = ingredientId2 }));
            mockIngredientService.Setup(p => p.GetCocktailIngredientsByCocktail(cocktailId))
        .Returns(Task.FromResult(testList));
            var options = TestUtilities.GetOptions(nameof(Should_UpdateCocktailCorrectly));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {

                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId});
                arrangeContext.SaveChanges();
            }
          

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService.Object);
                await sut.UpdateCocktailAsync(cocktailId, cocktailNewName, description, primaryIngredients, primaryIngredients, null);
                Assert.AreEqual(cocktailNewName, assertContext.Cocktails.First().Name);            
                Assert.AreEqual(description, assertContext.Cocktails.First().Description);
            }
        }
    }
}
