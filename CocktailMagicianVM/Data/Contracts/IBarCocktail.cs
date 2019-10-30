using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface IBarCocktail
    {
        int BarId { get; set; }
        Bar Bar { get; set; }
        int CocktailId { get; set; }
        Cocktail Cocktail { get; set; }
    }
}
