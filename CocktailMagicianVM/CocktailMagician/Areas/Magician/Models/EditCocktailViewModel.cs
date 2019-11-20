using Data.Contracts;
using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Magician.Models
{
    public class EditCocktailViewModel
    {
        public EditCocktailViewModel(ICocktail cocktail, IList<Ingredient> ingredients)
        {
            Id = cocktail.Id;
            Name = cocktail.Name;
            Description = cocktail.Description;
            MainIngredients = ingredients.Where(p => p.Primary == 1).Select(p => p.Name).ToList();
            Ingredients = ingredients.Where(p => p.Primary == 0).Select(p => p.Name).ToList();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> MainIngredients { get; set; }
        public List<string> Ingredients { get; set; }
        public string Description { get; set; }

    }
}
