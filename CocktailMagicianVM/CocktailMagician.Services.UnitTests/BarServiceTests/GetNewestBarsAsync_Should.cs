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

namespace CocktailMagician.Services.UnitTests.BarServiceTests
{
    [TestClass]
   public class GetNewestBarsAsync_Should
    {
        [TestMethod]
        public async Task ReturnNewestBarsCorrectly()
        {
            //arrange

            string testBarName1 = "TestName1";
            string testBarName2 = "TestName2";
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnNewestBarsCorrectly));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Bars.Add(new Bar() { Name = testBarName1 });
                arrangeContext.Bars.Add(new Bar() { Name = testBarName2 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                var bars = await sut.GetNewestBarsAsync();
                Assert.AreEqual(bars[0].Name, testBarName1);
                Assert.AreEqual(bars[1].Name, testBarName2);


            }
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "No bars exist.")]
        public async Task ThrowArgumentNullException_WhenNoBarsExist()
        {

            //arrange
            int testId = 9;
            int testIdFail = 8;
            string testBarName1 = "TestName1";
            string testBarName2 = "TestName2";
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentNullException_WhenNoBarsExist));

            //using (var arrangeContext = new CocktailDatabaseContext(options))
            //{

            //    arrangeContext.Bars.Add(new Bar() { Name = testBarName1, Id = testId });
            //    arrangeContext.Bars.Add(new Bar() { Name = testBarName2 });
            //    arrangeContext.SaveChanges();
            //}

            using (var assertContext = new CocktailDatabaseContext(options))
            {

                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                var bars = await sut.GetNewestBarsAsync();

            }


        }
    }
}
