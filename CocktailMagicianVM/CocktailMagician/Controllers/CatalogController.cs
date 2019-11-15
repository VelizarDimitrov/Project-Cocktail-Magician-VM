using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IAccountService aService;
        private readonly IBarService barService;
        private readonly ICocktailService cService;

        // Move actions to Bar and Coctail controllers
        public CatalogController(IAccountService aService,IBarService barService, ICocktailService cService)
        {
            this.aService = aService;
            this.barService = barService;
            this.cService = cService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBarPhoto(int id)
        {
            var picture = await barService.FindBarPhotoAsync(id);

            return File(picture, "image/png");
        }
        [HttpGet]
        public async Task<IActionResult> GetCocktailPhoto(int id)
        {
            var picture = await cService.FindCocktailPhotoAsync(id);

            return File(picture, "image/png");
        }
        public async Task<IActionResult> BarSearchResults(string keyword, string criteria, string order, string page, string rating, string sortOrder)
        {
            Tuple<IList<Bar>, bool> searchResults;
            var model = new BarSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                SelectedCriteria = criteria,
                SelectedOrderBy = order == null ? "" : order,
                Searched = true,
                Page = int.Parse(page)
            };

            searchResults = await barService.FindBarsForCatalogAsync(model.Keyword, model.SelectedCriteria, model.Page, model.SelectedOrderBy, rating, sortOrder);

            foreach (var bar in searchResults.Item1)
            {
                model.Bars.Add(new BarViewModel(bar));
            }
            model.LastPage = searchResults.Item2;
            return PartialView("_BarSearchView", model);
        }
        public async Task<IActionResult> CocktailSearchResults(string keyword, string criteria, string order, string page, string rating, string sortOrder, string mainIngredient)
        {
            Tuple<IList<Cocktail>, bool> cocktails;
            var model = new CocktailSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                SelectedCriteria = criteria,
                SelectedOrderBy = order == null ? "" : order,
                Searched = true,
                Page = int.Parse(page)
            };
            var ing = mainIngredient == null ? "" : mainIngredient;

            cocktails = await cService.FindCocktailsForCatalogAsync(model.Keyword, model.SelectedCriteria, model.Page, model.SelectedOrderBy, rating, sortOrder, ing);

            foreach (var cocktail in cocktails.Item1)
            {
                model.Cocktails.Add(new CocktailViewModel(cocktail));
            }
            model.LastPage = cocktails.Item2;
            return PartialView("_CocktailSearchView", model);
        }
        [HttpGet]
        public async Task<IActionResult> BarSearch()
        {
            var vm = new BarCatalogViewModel();
            var bars = await barService.GetNewestBarsAsync();
            foreach (var bar in bars)
            {
                vm.NewestBars.Add(new BarViewModel(bar));
            }
            return View("BarCatalog", vm);
        }
        [HttpGet]
        public async Task<IActionResult> CocktailSearch()
        {
            var ingredients = await cService.GetAllMainIngredients();
            var vm = new CocktailCatalogViewModel(ingredients);
            return View("CocktailCatalog", vm);
        }
    }
}