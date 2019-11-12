using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class CocktailIngredient:ICocktailIngredient
    {
     public  int IngredientId { get; set; }
     public  Ingredient Ingredient { get; set; }
     public  int CocktailId { get; set; }
     public  Cocktail Cocktail { get; set; }
     public string CocktailName { get; set; }
     public string IngredientName { get; set; }
    }
}
