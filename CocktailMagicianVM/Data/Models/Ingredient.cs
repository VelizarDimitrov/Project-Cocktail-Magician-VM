using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class Ingredient : IIngredient
    {
        public Ingredient()
        {
            Cocktails = new List<CocktailIngredient>(); 
        }

        public  int Id { get; set; }
        public  string Name { get; set; }
        public byte Primary { get; set; }
        public ICollection<CocktailIngredient> Cocktails { get; set; }
    }
}
