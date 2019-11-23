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

namespace CocktailMagician.Services.UnitTests.IngredientServiceTests
{
    [TestClass]
    public class FindIngredientsForCatalogAsync_Should
    {
        [TestMethod]
        public async Task ReturnCorrectAmount()
        {
            var keyword = "testName";
            var pageSize = 5;
            var page = 1;

            var options = TestUtilities.GetOptions(nameof(ReturnCorrectAmount));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 1, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 2, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 3, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 4, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 6, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 5, Name = keyword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var result = await sut.FindIngredientsForCatalogAsync(keyword, page, pageSize);
                Assert.AreEqual(pageSize, result.Item1.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsLastPage()
        {
            var ingredientCount = 6;
            var keyword = "testName";
            var pageSize = 5;
            var countExpected = ingredientCount % pageSize;
            var lastPage = ingredientCount / pageSize + 1;

            var options = TestUtilities.GetOptions(nameof(ReturnsLastPage));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 1, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 2, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 3, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 4, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 6, Name = keyword });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 5, Name = keyword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var result = await sut.FindIngredientsForCatalogAsync(keyword, lastPage, pageSize);
                Assert.AreEqual(countExpected, result.Item1.Count);
            }
        }
        [TestMethod]
        public async Task ReturnsCorrectIngredient()
        {
            var keyword = "testName";
            var pageSize = 5;

            var options = TestUtilities.GetOptions(nameof(ReturnsCorrectIngredient));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 1, Name = "se taq" });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 2, Name = keyword });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var result = await sut.FindIngredientsForCatalogAsync(keyword, 1, pageSize);
                Assert.AreEqual(keyword, result.Item1[0].Name);
                Assert.AreEqual(1, result.Item1.Count);
            }
        }

        [TestMethod]
        public async Task ReturnsEmptyList()
        {
            var keyword = "testName";
            var pageSize = 5;

            var options = TestUtilities.GetOptions(nameof(ReturnsEmptyList));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 1, Name = "se taq" });
                arrangeContext.Ingredients.Add(new Ingredient() { Id = 2, Name = "se taq" });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new IngredientService(assertContext);
                var result = await sut.FindIngredientsForCatalogAsync(keyword, 1, pageSize);
                Assert.IsTrue(!result.Item1.Any());
            }
        }
    }
}
