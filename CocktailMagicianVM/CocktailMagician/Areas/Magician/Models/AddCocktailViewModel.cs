using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string IngString { get; set; }
        [Required]
        public string MainIngString { get; set; }
        public IList<string> MainIngredients { get; set; }
        public IList<string> Ingredients { get; set; }
    }
}
