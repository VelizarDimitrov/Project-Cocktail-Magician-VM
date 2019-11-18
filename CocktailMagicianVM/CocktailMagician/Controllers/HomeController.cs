using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CocktailMagician.Models;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICityService cityService;
        private readonly IBarService barService;
        private readonly ICocktailService cService;

        public HomeController(ICityService cityService, IBarService barService, ICocktailService cService)
        {
            this.cityService = cityService;
            this.barService = barService;
            this.cService = cService;
        }
        public async Task<IActionResult> Index()
        {
            List<BarViewModel> topBarsVM = new List<BarViewModel>();
            List<CocktailViewModel> topCocktailsVM = new List<CocktailViewModel>();

            var topBars = await barService.FindBarsForCatalogAsync("", "", 1, "Rating", "0;5", "Descending", 6);
            var topCocktails = await cService.FindCocktailsForCatalogAsync("", "", 1, "Rating", "0;5", "Descending", "", 5);

            foreach (var bar in topBars.Item1)
            {
                topBarsVM.Add(new BarViewModel(bar));
            }
            foreach (var cocktail in topCocktails.Item1)
            {
                topCocktailsVM.Add(new CocktailViewModel(cocktail));
            }

            var vm = new IndexViewModel()
            {
                Bars = topBarsVM,
                Cocktails = topCocktailsVM
            };
            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GetAllCities()
        {
            var cities = (await cityService.GetAllCityNamesAsync()).ToArray();
            return Json(cities);
        }
    }
}
