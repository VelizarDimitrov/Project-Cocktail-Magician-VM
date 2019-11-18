using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
    public interface ICocktail
    {
        int Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }
        CocktailPhoto Photo { get; set; }
        ICollection<CocktailRating> Ratings { get; set; }
        ICollection<CocktailComment> Comments { get; set; }
        ICollection<BarCocktail> Bars { get; set; }
        ICollection<CocktailIngredient> Ingredients { get; set; }
        ICollection<UserCocktail> FavoritedBy { get; set; }
        double AverageRating { get; set; }
        byte Hidden { get; set; }
    }
}
