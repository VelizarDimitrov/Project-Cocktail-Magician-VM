using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailIngredientViewModel
    {
        public CocktailIngredientViewModel(ICocktailIngredient cocktailIngredient)
        {
            IngredientName = cocktailIngredient.IngredientName;
            IngredientId = cocktailIngredient.IngredientId;
            CocktailName = cocktailIngredient.CocktailName;
            CocktailId = cocktailIngredient.CocktailId;
        }
        public string IngredientName { get; set; }
        public string CocktailName { get; set; }
        public int IngredientId { get; set; }
        public int CocktailId { get; set; }
    }
}
