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

namespace CocktailMagician.Services.UnitTests.BarServiceTests
{
    [TestClass]
    public class FindBarByIdAsync_Should
    {


        [TestMethod]
        public async Task ReturnCorrectBarFromId()
        {

            //arrange
            int testId = 1;
            string testBarName1 = "TestName1";
            string testBarName2 = "TestName2";
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ReturnCorrectBarFromId));
            
            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                
                arrangeContext.Bars.Add(new Bar() { Name = testBarName1, });
                arrangeContext.Bars.Add(new Bar() { Name = testBarName2 });
                arrangeContext.SaveChanges();
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {

                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                var bar = await sut.FindBarByIdAsync(testId);
                Assert.AreEqual("testName1", bar.Name);
            }


        }
    }
}
