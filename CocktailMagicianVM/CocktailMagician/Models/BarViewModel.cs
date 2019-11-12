using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarViewModel
    {
        public BarViewModel(IBar bar)
        {
            StringBuilder stringBuilder = new StringBuilder();
            if (bar.Description.Length<256)
                stringBuilder.Append(bar.Description.Substring(0));
            else
                stringBuilder.Append(bar.Description.Substring(0,255));
            stringBuilder.Append("...");
            Id = bar.Id;
            Name = bar.Name;
            Address = bar.Address;
            AverageRating = bar.AverageRating;
            ShortDescription = stringBuilder.ToString();
            Description = bar.Description;
            RatingsCount = bar.Ratings.Count();
            City = bar.City.Name;
            Country = bar.Country.Name;
            Ratings = new BarRatingListViewModel(bar.Ratings);
            Comments = new BarCommentListViewModel(bar.Comments);
            Cocktails = new BarCocktailListViewModel(bar.Cocktails);
            FavoritedBy = new UserBarListViewModel(bar.FavoritedBy);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public double? AverageRating { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int RatingsCount { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public BarRatingListViewModel Ratings { get; set; }
        public BarCommentListViewModel Comments { get; set; }
        public BarCocktailListViewModel Cocktails { get; set; }
        public UserBarListViewModel FavoritedBy { get; set; }
    }
}
