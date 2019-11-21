using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Areas.Magician.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Areas.Magician.Controllers
{
    [Area("Magician")]
    [Authorize(Roles = "Cocktail Magician")]
    public class IngredientController : Controller
    {
        private readonly IIngredientService ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            this.ingredientService = ingredientService;
        }

        public IActionResult ManageIngredients()
        {
            return View("Ingredients");
        }
        public async Task<IActionResult> IngredientSearchResults(string keyword, string page, string pageSize)
        {
            Tuple<IList<Ingredient>, bool> ingredients;
            var model = new IngredientSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            ingredients = await ingredientService.FindIngredientsForCatalogAsync(model.Keyword, model.Page, int.Parse(pageSize));

            foreach (var ingredient in ingredients.Item1)
            {
                model.Ingredients.Add(new IngredientViewModel(ingredient));
            }
            model.LastPage = ingredients.Item2;
            return PartialView("_MagicianIngredientsView", model);
        }
        [HttpGet]
        public async Task<IActionResult> EditIngredient(string ingredientId)
        {
            var id = int.Parse(ingredientId);
            var ingredient = await ingredientService.FindIngredientByIdAsync(id);
            var vm = new IngredientViewModel(ingredient);
            return View("EditIngredient", vm);
        }
        public async Task<IActionResult> CheckIfNameAvailable(string ingredientname)
        {
            var ingredients = await ingredientService.FindIngredientsByNameAsync(ingredientname);
            var ingredientPrimes = new List<string>();
            foreach (var ingredient in ingredients)
            {
                if (ingredient.Primary==1)
                {
                    ingredientPrimes.Add("primary");
                }
                else
                {
                    ingredientPrimes.Add("secondary");
                }
            }
            return Json(ingredientPrimes.ToArray());
        }
        [HttpGet]
        public IActionResult AddIngredient()
        {
            var vm = new IngredientViewModel();
            return View("AddIngredient", vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddIngredient(IngredientViewModel ingredient)
        {
            byte prime;
            if (ingredient.Primary=="one")
            {
                prime = 1;
            }
            else
            {
                prime = 0;
            }
            await ingredientService.CreateIngredientAsync(ingredient.Name, prime);
            return RedirectToAction("ManageIngredients");
        }
        [HttpPost]
        public async Task<IActionResult> RemoveIngredient(string ingredientId)
        {
            await ingredientService.RemoveIngredientAsync(int.Parse(ingredientId));
            return RedirectToAction("ManageIngredients");
        }
    }
}