using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
   public class CocktailRating:ICocktailRating
    {
     public  int Rating { get; set; }
     public  int UserId { get; set; }
     public  User User { get; set; }
     public  int CocktailId { get; set; }
     public  Cocktail Cocktail { get; set; }
        public string UserUserName { get; set; }
        public string CocktailName { get; set; }
    }
}
