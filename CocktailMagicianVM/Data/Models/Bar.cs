using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Bar : IBar
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public int? CityId { get; set; }
        public City City { get; set; }
        public int? CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<BarRating> Ratings { get; set; }
        public ICollection<BarComment> Comments { get; set; }
        public ICollection<BarCocktail> Cocktails { get; set; }
        public ICollection<UserBar> FavoritedBy { get; set; }
    }
}
