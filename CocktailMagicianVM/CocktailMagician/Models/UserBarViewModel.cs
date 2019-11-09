using Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class UserBarViewModel
    {
        public UserBarViewModel(IUserBar userBar)
        {
            UserId = userBar.UserId;
            UserName = userBar.User.UserName;
            BarId = userBar.BarId;
            BarName = userBar.Bar.Name;
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BarId { get; set; }
        public string BarName { get; set; }
    }
}
