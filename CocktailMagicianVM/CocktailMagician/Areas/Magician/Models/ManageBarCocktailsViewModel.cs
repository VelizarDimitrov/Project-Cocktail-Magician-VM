using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Areas.Magician.Models
{
    public class ManageBarCocktailsViewModel
    {
        public ManageBarCocktailsViewModel(int id)
        {
            BarId = id;
        }
        public int BarId { get; set; }
    }
}
