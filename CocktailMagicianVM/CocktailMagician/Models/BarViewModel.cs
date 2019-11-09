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
            Id = bar.Id;
            Name = bar.Name;
            Address = bar.Address;
            //for (int i = 0; i <= 256; i++)
            //{
            //    if (i==0)
            //    {
            //        ShortDescription = "" + Description[i];
            //    }
            //    else
            //    {
            //    ShortDescription = ShortDescription + Description[i];
            //    }
            //    if (i==256)
            //    {
            //        ShortDescription = ShortDescription + "...";
            //    }
            //}
            if (bar.Ratings.Count == 0)
            {
                this.AverageRating = 0;
            }
            else
            {
                this.AverageRating = Math.Round(bar.Ratings.Average(p => p.Rating), 1);
            }
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i <= 256 && i<bar.Description.Length ; i++)
            {
                stringBuilder.Append(bar.Description[i]);
               
            }
            stringBuilder.Append("...");
            ShortDescription = stringBuilder.ToString();
            Description = bar.Description;
            RatingsCount = bar.Ratings.Count();
            BarCover = bar.BarCover;
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
        public byte[] BarCover { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public BarRatingListViewModel Ratings { get; set; }
        public BarCommentListViewModel Comments { get; set; }
        public BarCocktailListViewModel Cocktails { get; set; }
        public UserBarListViewModel FavoritedBy { get; set; }
    }
}
