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

namespace CocktailMagician.Services.UnitTests.NotificationServiceTests
{
    [TestClass]
    public class CityNotificationAsync_Should
    {
        [TestMethod]
        public async Task CreatesAndSendsCNotification()
        {
            string barName = "Bar";
            string cityName = "City";
            int userId = 1;

            var options = TestUtilities.GetOptions(nameof(CreatesAndSendsCNotification));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { City = cityName, Id = userId });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new NotificationService(actContext);
                await sut.CityNotificationAsync(barName, cityName);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.Include(p=>p.Notifications).First();
                Assert.IsNotNull(user.Notifications.FirstOrDefault());
            }
        }
    }
}
