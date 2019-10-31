using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface IBar
    {
        int Id { get; set; }
        string Name { get; set; }
        string Address { get; set; }
        string Description { get; set; }
        int? CityId { get; set; }
        City City { get; set; }
        int? CountryId { get; set; }
        Country Country { get; set; }
        ICollection<BarRating> Ratings { get; set; }
        ICollection<BarComment> Comments { get; set; }
        ICollection<BarCocktail> Cocktails { get; set; }
        ICollection<UserBar> FavoritedBy { get; set; }
    }
}
