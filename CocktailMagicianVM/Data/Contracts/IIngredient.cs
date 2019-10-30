using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
   public interface IIngredient
    {
        int Id { get; set; }
        string Name { get; set; }
        byte Primary { get; set; }
        ICollection<CocktailIngredient> Cocktails { get; set; }
    }
}
