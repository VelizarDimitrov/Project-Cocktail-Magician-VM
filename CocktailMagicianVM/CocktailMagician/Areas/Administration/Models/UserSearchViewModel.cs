using CocktailMagician.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Administration.Models
{
    public class UserSearchViewModel
    {
        public UserSearchViewModel()
        {
            Users = new List<UserViewModel>();
        }
        public bool Searched { get; set; }
        public string SelectedCriteria { get; set; }
        public string SelectedOrderBy { get; set; }
        public int Page { get; set; }
        public string Keyword { get; set; }
        public bool LastPage { get; set; }
        public List<UserViewModel> Users { get; set; }
    }
}

