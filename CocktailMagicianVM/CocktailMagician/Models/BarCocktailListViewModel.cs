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
            foreach (var barCocktail in barCocktails)
            {
                BarCocktails.Add(new BarCocktailViewModel(barCocktail));
            }
        }
        public List<BarCocktailViewModel> BarCocktails { get; set; }
    }
}
