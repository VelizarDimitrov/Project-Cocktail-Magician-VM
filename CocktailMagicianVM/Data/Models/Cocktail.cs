using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class Cocktail:ICocktail
    {
        public Cocktail()
        {
            Ratings = new List<CocktailRating>();
            Comments = new List<CocktailComment>();
            Bars = new List<BarCocktail>();
            Ingredients = new List<CocktailIngredient>();
            FavoritedBy = new List<UserCocktail>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CocktailPhoto Photo { get; set; }
        public ICollection<CocktailRating> Ratings { get; set; }
        public ICollection<CocktailComment> Comments { get; set; }
        public ICollection<BarCocktail> Bars { get; set; }
        public ICollection<CocktailIngredient> Ingredients { get; set; }
        public ICollection<UserCocktail> FavoritedBy { get; set; }
        public double AverageRating { get; set; }
        public byte Hidden { get; set; }
    }
}
