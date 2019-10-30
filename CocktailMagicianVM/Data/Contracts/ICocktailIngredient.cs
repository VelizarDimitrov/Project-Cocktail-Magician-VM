using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface ICocktailIngredient
    {
        string Comment { get; set; }
        int IngredientId { get; set; }
        Ingredient Ingredient { get; set; }
        int CocktailId { get; set; }
        Cocktail Cocktail { get; set; }
    }
}
