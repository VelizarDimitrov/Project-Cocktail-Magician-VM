using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarRatingViewModel
    {
        public BarRatingViewModel(IBarRating barRating)
        {
            UserName = barRating.User.UserName;
            BarName = barRating.Bar.Name;
            Rating = barRating.Rating;
            UserId = barRating.UserId;
            BarId = barRating.BarId;
        }
        public string UserName { get; set; }
        public string BarName { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int BarId { get; set; }
    }
}
