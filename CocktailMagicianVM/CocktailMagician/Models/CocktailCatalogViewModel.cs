using Data.Contracts;
using Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CocktailMagician.Models
{
    public class CocktailCatalogViewModel
    {
        public CocktailCatalogViewModel(ICollection<Ingredient> mainIngredients)
        {
            this.Criterias = new List<SelectListItem>
            {
                new SelectListItem("Name", "Name"),
                new SelectListItem("Bar", "Bar"),
                new SelectListItem("Ingredient", "Ingredient")
            };
            this.MainIngredients = new List<SelectListItem>();
            foreach (var ingredient in mainIngredients)
            {
                this.MainIngredients.Add(new SelectListItem(ingredient.Name,ingredient.Name));
            }
        }
        public List<SelectListItem> MainIngredients { get; set; }
        public List<SelectListItem> Criterias { get; set; }
    }
}
