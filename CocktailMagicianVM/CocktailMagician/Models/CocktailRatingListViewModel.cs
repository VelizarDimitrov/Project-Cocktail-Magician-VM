using Data.Contracts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailRatingListViewModel
    {
        public CocktailRatingListViewModel(ICollection<CocktailRating> cocktailRatings)
        {
            this.CocktailRatings = new List<CocktailRatingViewModel>();
            foreach (var cocktail in cocktailRatings)
            {
                CocktailRatings.Add(new CocktailRatingViewModel(cocktail));
            }
        }
        public List<CocktailRatingViewModel> CocktailRatings { get; set; }
    }
}
