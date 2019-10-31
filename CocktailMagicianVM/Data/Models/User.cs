using Data.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Models
{
    public class User : IUser
    {
        public User()
        {
        }

        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Required]
        public string Password { get; set; }
        public byte[] UserPhoto { get; set; }
        public string AccountStatus { get; set; }
        public string AccountType { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public ICollection<BarRating> BarRatings { get; set; }
        public ICollection<BarComment> BarComments { get; set; }
        public ICollection<CocktailRating> CocktailRatings { get; set; }
        public ICollection<CocktailComment> CocktailComments { get; set; }
        public ICollection<UserBar> FavoriteBars { get; set; }
        public ICollection<Notification> Notifications { get; set; }

    }
}
