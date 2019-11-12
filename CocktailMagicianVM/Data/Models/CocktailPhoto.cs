using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class CocktailPhoto : ICocktailPhoto
    {
        public int Id { get; set; }
        public byte[] CocktailCover { get; set; }
        public int CocktailId { get; set; }
        public Cocktail Cocktail{ get; set; }
    }
}
