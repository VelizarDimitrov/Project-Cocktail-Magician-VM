using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Magician.Models
{
    public class AddCocktailViewModel
    {
        public AddCocktailViewModel()
        {
            MainIngredients = new List<string>();
            Ingredients = new List<string>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public IList<string> MainIngredients { get; set; }
        public IList<string> Ingredients { get; set; }
    }
}
