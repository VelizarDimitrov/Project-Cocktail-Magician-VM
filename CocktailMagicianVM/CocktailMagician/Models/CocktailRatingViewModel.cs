using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailRatingViewModel
    {
        public CocktailRatingViewModel(ICocktailRating cocktailRating)
        {
            UserName = cocktailRating.UserUserName;
            CocktailName = cocktailRating.CocktailName;
            Rating = cocktailRating.Rating;
            UserId = cocktailRating.UserId;
            CocktailId = cocktailRating.CocktailId;
        }
        public string UserName { get; set; }
        public string CocktailName { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public int CocktailId { get; set; }
    }
}
