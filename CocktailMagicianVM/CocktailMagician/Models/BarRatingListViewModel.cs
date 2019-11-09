using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarRatingListViewModel
    {
        public BarRatingListViewModel(ICollection<BarRating> barRatings)
        {
            this.BarRatings = new List<BarRatingViewModel>();
            foreach (var barRating in barRatings)
            {
                BarRatings.Add(new BarRatingViewModel(barRating));
            }
        }
        public List<BarRatingViewModel> BarRatings { get; set; }
    }
}
