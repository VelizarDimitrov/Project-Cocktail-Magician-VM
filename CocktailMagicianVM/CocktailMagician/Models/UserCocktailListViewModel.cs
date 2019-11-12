using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class UserCocktailListViewModel
    {
        public UserCocktailListViewModel(ICollection<UserCocktail> userCocktails)
        {
            this.UserCocktails = new List<UserCocktailViewModel>();
            foreach (var userCocktail in userCocktails)
            {
                UserCocktails.Add(new UserCocktailViewModel(userCocktail));
            }
        }
        public List<UserCocktailViewModel> UserCocktails { get; set; }
    }
}
