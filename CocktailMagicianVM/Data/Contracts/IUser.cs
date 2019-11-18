using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
    public interface IUser
    {
        int Id { get; set; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Password { get; set; }
        string AccountStatus { get; set; }
        string AccountType { get; set; }
        DateTime? LastLogIn { get; set; }
        int? CountryId { get; set; }
        Country Country { get; set; }
        int? CityId { get; set; }
        City City { get; set; }
        UserPhoto UserPhoto { get; set; }
        ICollection<BarRating> BarRatings { get; set; }
        ICollection<BarComment> BarComments { get; set; }
        ICollection<CocktailRating> CocktailRatings { get; set; }
        ICollection<CocktailComment> CocktailComments { get; set; }
        ICollection<UserBar> FavoriteBars { get; set; }
        ICollection<UserCocktail> FavoriteCocktails { get; set; }
        ICollection<Notification> Notifications { get; set; }
    }
}
