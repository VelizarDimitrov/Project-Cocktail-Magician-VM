using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarCocktailListViewModel
    {
        public BarCocktailListViewModel(ICollection<BarCocktail> barCocktails)
        {
            this.BarCocktails = new List<BarCocktailViewModel>();
            var reversed = barCocktails.Reverse();
            foreach (var barCocktail in reversed)
            {
                BarCocktails.Add(new BarCocktailViewModel(barCocktail));
            }
        }
        public List<BarCocktailViewModel> BarCocktails { get; set; }
    }
}
