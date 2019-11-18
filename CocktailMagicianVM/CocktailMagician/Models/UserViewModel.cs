using Data.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class UserViewModel
    {
        public UserViewModel(IUser user)
        {
            Id = user.Id;
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password;
            AccountStatus = user.AccountStatus;
            AccountType = user.AccountType;
            Country = user.Country.Name;
            City = user.City.Name;
            BarRatings = new BarRatingListViewModel(user.BarRatings);
            BarComments = new BarCommentListViewModel(user.BarComments);
            CocktailRatings = new CocktailRatingListViewModel(user.CocktailRatings);
            CocktailComments = new CocktailCommentListViewModel(user.CocktailComments);
            FavoriteBars = new UserBarListViewModel(user.FavoriteBars);
            FavoriteCocktails = new UserCocktailListViewModel(user.FavoriteCocktails);
            Notifications = new NotificationListViewModel(user.Notifications);
            LastLogIn = user.LastLogIn;
            LastPage = true;
        }
        public UserViewModel()
        {

        }

        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        public string AccountStatus { get; set; }
        public string AccountType { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        [Display(Name = "Avatar")]
        public BarRatingListViewModel BarRatings { get; set; }
        public BarCommentListViewModel BarComments { get; set; }
        public CocktailRatingListViewModel CocktailRatings { get; set; }
        public CocktailCommentListViewModel CocktailComments { get; set; }
        public UserBarListViewModel FavoriteBars { get; set; }
        public UserCocktailListViewModel FavoriteCocktails { get; set; }
        public NotificationListViewModel Notifications { get; set; }
        public DateTime? LastLogIn { get; set; }
        public int Page { get; set; }
        public bool LastPage { get; set; }
    }
}
