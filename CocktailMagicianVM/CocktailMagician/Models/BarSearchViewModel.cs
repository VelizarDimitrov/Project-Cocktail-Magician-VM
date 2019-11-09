using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarSearchViewModel
    {
        public BarSearchViewModel()
        {
            Bars = new List<BarViewModel>();
        }
        public bool Searched { get; set; }
        public string SelectedCriteria { get; set; }
        public string SelectedOrderBy { get; set; }
        public int Page { get; set; }
        public string Keyword { get; set; }
        public bool LastPage { get; set; }
        public List<BarViewModel> Bars { get; set; }
    }
}
