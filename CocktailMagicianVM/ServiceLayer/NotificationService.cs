using Data;
using Data.Contracts;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer
{
    public class NotificationService : INotificationService
    {
        private readonly CocktailDatabaseContext dbContext;

        public NotificationService(CocktailDatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task CityNotificationAsync(string barName, string cityName)
        {
            var users = await dbContext.Users.Where(p => p.City.ToLower() == cityName.ToLower()).ToListAsync();
            foreach (var user in users)
            {
                var notification = new Notification()
                {
                    Name = "New Bar",
                    Text = $"A new bar called {barName} has opened in {cityName}",
                    User = user
                };
                await dbContext.Notifications.AddAsync(notification);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task FavBarNotificationAsync(string barName, string cocktailName)
        {
            var users = await dbContext.Users
                .Where(p => p.FavoriteBars.Any(x=>x.BarName.ToLower()==barName.ToLower()))
                .ToListAsync();

            foreach (var user in users)
            {
                var notification = new Notification()
                {
                    Name = "New Cocktail Available",
                    Text = $"A new cocktail named {cocktailName} is available at your favourite bar {barName}",
                    User = user
                };
                await dbContext.Notifications.AddAsync(notification);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task FavCocktailNotificationAsync(string barName, string cocktailName, string cityName)
        {
            var users = await dbContext.Users
                .Where(p=>p.FavoriteCocktails
                .Any(xp=>xp.CocktailName.ToLower()==cocktailName.ToLower()) && p.City.ToLower()==cityName.ToLower())
                .ToListAsync();

            foreach (var user in users)
            {
                var notification = new Notification()
                {
                    Name = "Favourite Cocktail Found Nearby",
                    Text = $"Your favourite cocktail {cocktailName} is available in the bar {barName} in {cityName}",
                    User = user
                };
                await dbContext.Notifications.AddAsync(notification);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
