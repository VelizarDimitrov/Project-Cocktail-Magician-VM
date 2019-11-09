using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class UserBarListViewModel
    {
        public UserBarListViewModel(ICollection<UserBar> userBars)
        {
            this.UserBars = new List<UserBarViewModel>();
            foreach (var userBar in userBars)
            {
                UserBars.Add(new UserBarViewModel(userBar));
            }
        }
        public List<UserBarViewModel> UserBars { get; set; }
    }
}
