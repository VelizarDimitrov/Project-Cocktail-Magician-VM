using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class Cocktail:ICocktail
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<CocktailRating> Ratings { get; set; }
        public ICollection<CocktailComment> Comments { get; set; }
        public ICollection<BarCocktail> Bars { get; set; }
        public ICollection<CocktailIngredient> Ingredients { get; set; }
    }
}
