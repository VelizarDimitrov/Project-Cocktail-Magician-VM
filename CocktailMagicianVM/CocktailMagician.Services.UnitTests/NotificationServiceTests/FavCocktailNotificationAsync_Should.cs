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
    public class FavCocktailNotificationAsync_Should
    {
        [TestMethod]
        public async Task CreatesAndSendsFCNotification()
        {
            string barName = "Bar";
            string cocktailName = "Cocktail";
            string cityName = "City";
            int userId = 1;
            int cocktailId = 1;

            var options = TestUtilities.GetOptions(nameof(CreatesAndSendsFCNotification));

            using (var arrangeContext = new CocktailDatabaseContext(options))
            {
                arrangeContext.Users.Add(new User() { Id = userId, City = cityName });
                arrangeContext.Cocktails.Add(new Cocktail() { Id = cocktailId });
                arrangeContext.SaveChanges();
                arrangeContext.UserCocktail.Add(new UserCocktail() { CocktailId = cocktailId, UserId = userId, CocktailName = cocktailName });
                arrangeContext.SaveChanges();
            }

            using (var actContext = new CocktailDatabaseContext(options))
            {
                var sut = new NotificationService(actContext);
                await sut.FavCocktailNotificationAsync(barName, cocktailName, cityName);
            }

            using (var assertContext = new CocktailDatabaseContext(options))
            {
                var user = assertContext.Users.Include(p => p.Notifications).First();
                Assert.IsNotNull(user.Notifications.FirstOrDefault());
            }
        }
    }
}
