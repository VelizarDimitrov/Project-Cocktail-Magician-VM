using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Contracts
{
    public interface INotificationService
    {
        Task FavBarNotificationAsync(string barName, string cocktailName);
        Task CityNotificationAsync(string barName, string cityName);
        Task FavCocktailNotificationAsync(string barName, string cocktailName, string cityName);
    }
}
