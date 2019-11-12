using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface ICocktailIngredient
    {
        int IngredientId { get; set; }
        Ingredient Ingredient { get; set; }
        int CocktailId { get; set; }
        Cocktail Cocktail { get; set; }
        string CocktailName { get; set; }
        string IngredientName { get; set; }
    }
}
