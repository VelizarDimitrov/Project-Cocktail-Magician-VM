using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Magician.Models
{
    public class IngredientSearchViewModel
    {
        public IngredientSearchViewModel()
        {
            Ingredients = new List<IngredientViewModel>();
        }
        public bool Searched { get; set; }
        public string SelectedCriteria { get; set; }
        public string SelectedOrderBy { get; set; }
        public int Page { get; set; }
        public string Keyword { get; set; }
        public bool LastPage { get; set; }
        public List<IngredientViewModel> Ingredients { get; set; }

    }
}
