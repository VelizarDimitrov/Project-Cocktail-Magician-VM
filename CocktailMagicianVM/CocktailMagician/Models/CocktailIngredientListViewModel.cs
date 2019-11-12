using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailIngredientListViewModel
    {
        public CocktailIngredientListViewModel(ICollection<CocktailIngredient> cocktailIngredients)
        {
            this.CocktailIngredients = new List<CocktailIngredientViewModel>();
            foreach (var cocktailIngredient in cocktailIngredients)
            {
                CocktailIngredients.Add(new CocktailIngredientViewModel(cocktailIngredient));
            }
        }
        public List<CocktailIngredientViewModel> CocktailIngredients { get; set; }
    }
}
