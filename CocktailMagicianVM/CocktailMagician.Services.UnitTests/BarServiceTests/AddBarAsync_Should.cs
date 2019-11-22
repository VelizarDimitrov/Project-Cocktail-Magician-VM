using Data;
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

namespace CocktailMagician.Services.UnitTests.BarServiceTests
{
    [TestClass]
   public class AddBarAsync_Should
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Bar name cannot be null or empty.")]
        public async Task ThrowArgumentException_NullBarname()
        {
        //ICountryService countryService, ICityService cityService, ICocktailService cocktailService, INotificationService nService

            //arrange
            string testBarName = null;
            byte[] coverPhoto=new byte[0];
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_NullBarname));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.AddBarAsync(testBarName, "se taq", "se taq", "se taq", "se taq", coverPhoto);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username cannot be null or empty.")]
        public async Task ThrowArgumentException_EmptyBarname()
        {
            //arrange
            string testBarName ="";
            byte[] coverPhoto = new byte[0];
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_NullBarname));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.AddBarAsync(testBarName, "se taq", "se taq", "se taq", "se taq", coverPhoto);
            }

        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Bar name cannot be null or empty.")]
        public async Task ThrowArgumentException_NullAddress()
        {
            //ICountryService countryService, ICityService cityService, ICocktailService cocktailService, INotificationService nService

            //arrange
            string testAddress = null;
            byte[] coverPhoto = new byte[0];
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_NullAddress));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.AddBarAsync("se taq", testAddress, "se taq", "se taq", "se taq", coverPhoto);
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Username cannot be null or empty.")]
        public async Task ThrowArgumentException_EmptyAddress()
        {
            //arrange
            string testAddress = "";
            byte[] coverPhoto = new byte[0];
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(ThrowArgumentException_EmptyAddress));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.AddBarAsync("se taq", testAddress, "se taq", "se taq", "se taq", coverPhoto);
            }

        }
        [TestMethod]
       
        public async Task Create_Bar()
        {
            //arrange
            string barName = "testName";
            byte[] coverPhoto = new byte[0];
            var mockCountryService = new Mock<ICountryService>().Object;
            var mockCityService = new Mock<ICityService>().Object;
            var mockCocktailService = new Mock<ICocktailService>().Object;
            var mockNotificationService = new Mock<INotificationService>().Object;
            var options = TestUtilities.GetOptions(nameof(Create_Bar));

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var sut = new BarService(assertContext, mockCountryService, mockCityService, mockCocktailService, mockNotificationService);
                await sut.AddBarAsync(barName, "se taq", "se taq", "se taq", "se taq", coverPhoto);
            }
            using (var assertContext = new CocktailDatabaseContext(options))
            {
                Assert.AreEqual(1, assertContext.Bars.Count());
                var bar = await assertContext.Bars.FirstOrDefaultAsync(u => u.Name == barName);
                Assert.IsNotNull(bar);
                
            }

        }
    }
}
