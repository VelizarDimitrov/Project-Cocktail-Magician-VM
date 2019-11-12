using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class BarCatalogViewModel
    {
        public BarCatalogViewModel()
        {
            this.Criterias = new List<SelectListItem>
            {
                new SelectListItem("Name", "Name"),
                new SelectListItem("Address", "Address"),
                new SelectListItem("City", "City")
            };
            this.NewestBars = new List<BarViewModel>();
        }
        public List<SelectListItem> Criterias { get; set; }
        public List<BarViewModel> NewestBars { get; set; }
    }
}
