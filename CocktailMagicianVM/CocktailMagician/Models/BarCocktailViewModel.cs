using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarCocktailViewModel
    {
        public BarCocktailViewModel(IBarCocktail barCocktail)
        {
            BarId = barCocktail.BarId;
            BarName = barCocktail.Bar.Name;
            CocktailId = barCocktail.CocktailId;
            CocktailName = barCocktail.Cocktail.Name;
        }
        public int BarId { get; set; }
        public string BarName { get; set; }
        public int CocktailId { get; set; }
        public string CocktailName { get; set; }
    }
}
