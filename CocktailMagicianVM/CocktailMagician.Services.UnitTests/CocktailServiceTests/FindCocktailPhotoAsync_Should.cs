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
   public class FindCocktailPhotoAsync_Should
    {
        [TestMethod]
        public async Task Should_ReturnCocktailPhotoCorrectlyFromGivenId()
        {
            //arrange
            string cocktailName = "testName";
            int cocktailId = 14;
            byte[] coverPhoto = new byte[0];
            string[] primaryIngredients = new string[1] { "test1" };
            var mockIngredientService = new Mock<IIngredientService>().Object;

            var options = TestUtilities.GetOptions(nameof(Should_ReturnCocktailPhotoCorrectlyFromGivenId));
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Cocktails.Add(new Cocktail() { Name = cocktailName,Id= cocktailId }); ;
                arrangeContext.SaveChanges();
            }
            using (var actContext = new CocktailDatabaseContext(options))
            {
                actContext.CocktailPhotos.Add(new CocktailPhoto() { CocktailCover = coverPhoto, Cocktail = actContext.Cocktails.First() });
                actContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new CocktailService(assertContext, mockIngredientService);
                var cocktailPhoto = await sut.FindCocktailPhotoAsync(cocktailId);
                Assert.IsNotNull(cocktailPhoto);
                Assert.AreEqual(assertContext.Cocktails.First().Photo.CocktailCover, cocktailPhoto.CocktailCover);
            }
        }
    }
}
