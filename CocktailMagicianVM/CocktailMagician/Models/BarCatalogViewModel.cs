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
            this.BarSearchViewModel = new BarSearchViewModel()
            {
                Searched = false
            };
            this.Criterias = new List<SelectListItem>
                { new SelectListItem("Name", "Name")
                , new SelectListItem("Address", "Address")
                , new SelectListItem("City", "City")};
            this.OrderBy = new List<SelectListItem>
            {
                new SelectListItem("","")
                , new SelectListItem("Name", "Name")
                , new SelectListItem("Address", "Address")
                , new SelectListItem("City", "City")
                , new SelectListItem("Rating", "Rating")
            };
        }
        public BarSearchViewModel BarSearchViewModel { get; set; }
        public List<SelectListItem> Criterias { get; set; }
        public List<SelectListItem> OrderBy { get; set; }
        public string SelectedCriteria { get; set; }
        public string SelectedOrderBy { get; set; }
        public int Page { get; set; }
        public string Keyword { get; set; }
        public bool LastPage { get; set; }
    }
}
