using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Contracts
{
    public interface ICocktailComment
    {
        int Id { get; set; }
        string Comment { get; set; }
        int UserId { get; set; }
        User User { get; set; }
        int CocktailId { get; set; }
        Cocktail Cocktail { get; set; }
        string CocktailName { get; set; }
        string UserName { get; set; }
        DateTime CreatedOn { get; set; }
    }
}
