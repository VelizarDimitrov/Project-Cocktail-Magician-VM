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
    public class FavBarNotificationAsync_Should
    {
        [TestMethod]
        public async Task CreatesAndSendsFBNotification()
        {
            string barName = "Bar";
            string cocktailName = "Cocktail";
            int userId = 1;
            int barId = 1;

            var options = TestUtilities.GetOptions(nameof(CreatesAndSendsFBNotification));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = userId });
                arrangeContext.Bars.Add(new Bar() { Id = barId });
                arrangeContext.SaveChanges();
                arrangeContext.UserBar.Add(new UserBar() { BarId = barId , UserId= userId, BarName= barName });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new NotificationService(actContext);
                await sut.FavBarNotificationAsync(barName, cocktailName);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.Include(p => p.Notifications).First();
                Assert.IsNotNull(user.Notifications.FirstOrDefault());
            }
        }
    }
}
