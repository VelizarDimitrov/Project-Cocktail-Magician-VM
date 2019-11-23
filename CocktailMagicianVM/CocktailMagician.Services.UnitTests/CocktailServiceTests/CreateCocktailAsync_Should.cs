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
   public class CreateCocktailAsync_Should
    {
        [TestMethod]
        public async Task Should_CorrectlyCreateCocktail()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients =new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 1))
             .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 0))
           .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 0))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 0 }));
            var options = TestUtilities.GetOptions(nameof(Should_CorrectlyCreateCocktail));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService.Object);
                await sut.CreateCocktailAsync(cocktailName,"se taq", primaryIngredients, primaryIngredients, coverPhoto);
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                Assert.AreEqual(1, assertContext.Cocktails.Count());
                var cocktail = await assertContext.Cocktails.FirstOrDefaultAsync(u => u.Name == cocktailName);
                Assert.IsNotNull(cocktail);

            }

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Primary ingredients list cannot be null.")]
        public async Task Should_ThrowArgumentNullException_WhenPrimaryIngredientIsNull()
        {
            //arrange
            string cocktailName = "testName";
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 1))
             .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 0))
           .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 0))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 0 }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenPrimaryIngredientIsNull));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService.Object);
                await sut.CreateCocktailAsync(cocktailName, "se taq", null, primaryIngredients, coverPhoto);
            }
          

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Cocktail name cannot be null or whitespace.")]
        public async Task Should_ThrowArgumentNullException_WhenNameIsNull()
        {
            //arrange
            string cocktailName = "testName";
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 1))
             .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 0))
           .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 0))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 0 }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenNameIsNull));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService.Object);
                await sut.CreateCocktailAsync(null, "se taq", primaryIngredients, primaryIngredients, coverPhoto);
            }


        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Cocktail name cannot be null or whitespace.")]
        public async Task Should_ThrowArgumentNullException_WhenNameIsWhitespace()
        {
            //arrange
            string cocktailName = "testName";
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>();
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 1))
             .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.CheckIfIngredientExistsAsync(primaryIngredients[0], 0))
           .Returns(Task.FromResult(true));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 1))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 1 }));
            mockIngredientService.Setup(p => p.GetIngredientByNameTypeAsync(primaryIngredients[0], 0))
         .Returns(Task.FromResult(new Ingredient() { Name = primaryIngredients[0], Primary = 0 }));
            var options = TestUtilities.GetOptions(nameof(Should_ThrowArgumentNullException_WhenNameIsWhitespace));
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService.Object);
                await sut.CreateCocktailAsync("", "se taq", primaryIngredients, primaryIngredients, coverPhoto);
            }


        }
    }
}
