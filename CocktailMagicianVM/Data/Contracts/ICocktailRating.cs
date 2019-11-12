using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
    public interface ICocktailRating
    {
        int Rating { get; set; }
        int UserId { get; set; }
        User User { get; set; }
        int CocktailId { get; set; }
        Cocktail Cocktail { get; set; }
        string UserUserName { get; set; }
        string CocktailName { get; set; }
    }
}
