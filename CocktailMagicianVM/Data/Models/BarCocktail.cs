using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class BarCocktail : IBarCocktail
    {
        public int BarId { get; set; }
        public Bar Bar { get; set; }
        public int CocktailId { get; set; }
        public Cocktail Cocktail { get; set; }
    }
}
