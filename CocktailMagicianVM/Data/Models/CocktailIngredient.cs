using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class CocktailIngredient:ICocktailIngredient
    {
     public  string Comment { get; set; }
     public  int IngredientId { get; set; }
     public  Ingredient Ingredient { get; set; }
     public  int CocktailId { get; set; }
     public  Cocktail Cocktail { get; set; }
    }
}
