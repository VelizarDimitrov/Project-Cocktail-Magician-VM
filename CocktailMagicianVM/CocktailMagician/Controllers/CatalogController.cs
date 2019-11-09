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

        public CatalogController(IAccountService aService,IBarService barService)
        {
            this.aService = aService;
            this.barService = barService;
        }
        [HttpGet]
        public async Task<IActionResult> GetBarPhoto(int id)
        {
            var picture = await barService.FindBarPhoto(id);

            return File(picture, "image/png");
        }
        public async Task<IActionResult> BarSearchResults(string keyword, string criteria, string order, string page, string rating)
        {
            var model = new BarSearchViewModel()
            {
                Keyword = keyword,
                SelectedCriteria = criteria,
                SelectedOrderBy = order,
                Searched = true
            };

            if (model.Keyword == null)
            {
                model.Keyword = "";
            }
            if (model.SelectedOrderBy == null)
            {
                model.SelectedOrderBy = "";
            }
            model.Page = int.Parse(page);
            Tuple<IList<Bar>, bool> tuple;
            switch (model.SelectedCriteria)
            {
                case "Name":
                    tuple = await barService.FindBarByNameAsync(model.Keyword, model.Page, model.SelectedOrderBy, rating);
                    break;
                case "Address":
                    tuple = await barService.FindBarByAddressAsync(model.Keyword, model.Page, model.SelectedOrderBy, rating);
                    break;
                case "City":
                    tuple = await barService.FindBarByCityAsync(model.Keyword, model.Page, model.SelectedOrderBy, rating);
                    break;
                default:
                    throw new ArgumentException();
            }
            IList<Bar> bars = tuple.Item1;
            foreach (var bar in bars)
            {
                model.Bars.Add(new BarViewModel(bar));
            }
            model.LastPage = tuple.Item2;
            return PartialView("_BarSearchView", model);
        }
        [HttpGet]
        public IActionResult BarSearch()
        {
            var vm = new BarCatalogViewModel();
            return View("BarCatalog", vm);
        }
    }
}