using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
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
   public class AddIngredientToCocktailAsync_Should
    {
        [TestMethod]
        public async Task Should_CorrectlyAddIngredientToCocktail()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            var options = TestUtilities.GetOptions(nameof(Should_CorrectlyAddIngredientToCocktail));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId }); ;
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(actContext, mockIngredientService.Object);
                await sut.AddIngredientToCocktailAsync(cocktailName, primaryIngredients[0],1);

            }
          

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Cocktail name cannot be null or whitespace.")]
        public async Task Should_ThrowArgumentNullException_WhenCocktailNameIsNull()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenCocktailNameIsNull));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId }); ;
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(actContext, mockIngredientService.Object);
                await sut.AddIngredientToCocktailAsync(null, primaryIngredients[0], 1);
            }


        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Cocktail name cannot be null or whitespace.")]
        public async Task Should_ThrowArgumentNullException_WhenCocktailNameIsWhitespace()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenCocktailNameIsWhitespace));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName, Id = cocktailId }); ;
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(actContext, mockIngredientService.Object);
                await sut.AddIngredientToCocktailAsync("", primaryIngredients[0], 1);
            }


        }
    }
}
