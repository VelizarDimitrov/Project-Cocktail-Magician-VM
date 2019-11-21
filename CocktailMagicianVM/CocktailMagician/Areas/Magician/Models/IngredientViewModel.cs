using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Magician.Models
{
    public class IngredientViewModel
    {
        public IngredientViewModel(IIngredient ingredient)
        {
            this.Id = ingredient.Id;
            this.Name = ingredient.Name;
            if (ingredient.Primary==1)
            {
                Primary = "one";
            }
            else
            {
                Primary = "zero";
            }
            CocktailCount = ingredient.Cocktails.Count();
        }
        public IngredientViewModel()
        {

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Primary { get; set; }
        
        public int CocktailCount { get; set; }
    }
}
