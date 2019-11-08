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

        public HomeController(ICityService cityService,IBarService barService)
        {
            this.cityService = cityService;
            this.barService = barService;
        }
    public IActionResult Index()
        {
            return View();
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
            var cities = (await cityService.GetAllCityNames()).ToArray();
            return Json(cities);
        }
        public async Task<IActionResult> GetAllBars(string cityName)
        {
            string[] bars;
            if (await cityService.CheckifCityNameIsCorrect(cityName))
            {
               bars = (await barService.GetBarsFromCity(cityName)).ToArray();
                
            }
            else
            {
               bars = (await barService.GetAllBarNames()).ToArray();

            }
            return Json(bars);
        }
    }
}
