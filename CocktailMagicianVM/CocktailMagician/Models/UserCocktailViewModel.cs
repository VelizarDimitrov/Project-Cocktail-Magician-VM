using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class UserCocktailViewModel
    {
        public UserCocktailViewModel(IUserCocktail userCocktail)
        {
            UserId = userCocktail.UserId;
            UserName = userCocktail.UserUserName;
            CocktailId = userCocktail.CocktailId;
            CocktailName = userCocktail.CocktailName;
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int CocktailId { get; set; }
        public string CocktailName { get; set; }
    }
}
