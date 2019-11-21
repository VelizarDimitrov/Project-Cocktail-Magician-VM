using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface INotificationService
    {
        Task FavBarNotification(string barName, string cocktailName);
        Task CityNotification(string barName, string cityName);
        Task FavCocktailNotification(string barName, string cocktailName, string cityName);
    }
}
