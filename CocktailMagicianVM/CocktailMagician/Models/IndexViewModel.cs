using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class IndexViewModel
    {
        public string BarName { get; set; }
        public List<CocktailViewModel> Cocktails { get; set; }
        public List<BarViewModel> Bars { get; set; }
    }
}
