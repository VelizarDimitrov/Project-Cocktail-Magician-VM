using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailViewModel
    {
        public CocktailViewModel(ICocktail cocktail)
        {
            StringBuilder stringBuilder = new StringBuilder();
            var ingredientsText = String.Join(", ", cocktail.Ingredients.Select(p => p.IngredientName));
            if (ingredientsText.Length < 53)
                stringBuilder.Append(ingredientsText.Substring(0));
            else
            {
                stringBuilder.Append(ingredientsText.Substring(0, 53));
                stringBuilder.Append("...");
            }
            Id = cocktail.Id;
            Name = cocktail.Name;
            AverageRating = cocktail.AverageRating;
            ShortDescription = stringBuilder.ToString();
            Description = cocktail.Description;
            RatingsCount = cocktail.Ratings.Count();
            Ratings = new CocktailRatingListViewModel(cocktail.Ratings);
            Comments = new CocktailCommentListViewModel(cocktail.Comments);
            Bars = new BarCocktailListViewModel(cocktail.Bars);
            FavoritedBy = new UserCocktailListViewModel(cocktail.FavoritedBy);
            Ingredients = new CocktailIngredientListViewModel(cocktail.Ingredients);
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public double? AverageRating { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int RatingsCount { get; set; }
        public CocktailRatingListViewModel Ratings { get; set; }
        public CocktailCommentListViewModel Comments { get; set; }
        public BarCocktailListViewModel Bars { get; set; }
        public UserCocktailListViewModel FavoritedBy { get; set; }
        public CocktailIngredientListViewModel Ingredients { get; set; }
    }
}
