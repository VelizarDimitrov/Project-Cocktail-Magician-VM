using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
    public interface ICocktailPhoto
    {
        int Id { get; set; }
        byte[] CocktailCover { get; set; }
        int CocktailId { get; set; }
        Cocktail Cocktail { get; set; }
    }
}
