using Data;
using Data.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ServiceLayer;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Services.UnitTests.CocktailServiceTests
{
    [TestClass]
   public class FindCocktailByNameAsync_Should
    {
        [TestMethod]
        public async Task Should_ReturnCocktailCorrectlyFromGivenName()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>().Object;
  
            var options = TestUtilities.GetOptions(nameof(Should_ReturnCocktailCorrectlyFromGivenName));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName }); ;
                arrangeContext.SaveChanges();
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService);
              var cocktail = await sut.FindCocktailByNameAsync(cocktailName);
                Assert.IsNotNull(cocktail);
                Assert.AreEqual(cocktailName, cocktail.Name);
            }
         

        }
    }
}
