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
        public async Task<IActionResult> BarSearchResults(string keyword, string criteria, string order, string page, string rating, string sortOrder)
        {
            Tuple<IList<Bar>, bool> tuple;
            var model = new BarSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                SelectedCriteria = criteria,
                SelectedOrderBy = order == null ? "" : order,
                Searched = true,
                Page = int.Parse(page)
            };
            switch (model.SelectedCriteria)
            {
                case "Name":
                    tuple = await barService.FindBarByNameAsync(model.Keyword, model.Page, model.SelectedOrderBy, rating, sortOrder);
                    break;
                case "Address":
                    tuple = await barService.FindBarByAddressAsync(model.Keyword, model.Page, model.SelectedOrderBy, rating, sortOrder);
                    break;
                case "City":
                    tuple = await barService.FindBarByCityAsync(model.Keyword, model.Page, model.SelectedOrderBy, rating, sortOrder);
                    break;
                default:
                    throw new ArgumentException();
            }
            foreach (var bar in tuple.Item1)
            {
                model.Bars.Add(new BarViewModel(bar));
            }
            model.LastPage = tuple.Item2;
            return PartialView("_BarSearchView", model);
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