using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class BarController : Controller
    {
        private readonly ICountryService countryService;
        private readonly ICityService cityService;
        private readonly IBarService barService;

        public BarController(ICountryService countryService, ICityService cityService, IBarService barService)
        {
            this.countryService = countryService;
            this.cityService = cityService;
            this.barService = barService;
        }
        public IActionResult AddBar()
        {
            var vm = new AddBarViewModel();
            return View("AddBar", vm);
        }
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = (await countryService.GetAllCountryNamesAsync()).ToArray();
            return Json(countries);
        }
        public async Task<IActionResult> GetAllCities(string cityName)
        {
            string[] cities;
            if (await countryService.CheckIfCountryNameIsCorrect(cityName))
            {
                cities = (await cityService.GetCitiesFromCountryAsync(cityName)).ToArray();

            }
            else
            {
                cities = new string[0];

            }
            return Json(cities);
        }
        [HttpPost]
        public async Task<IActionResult> AddBar(AddBarViewModel barModel, IFormFile file)
        {
            byte[] barPhoto;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                barPhoto = stream.ToArray();

            }
            try
            {
                await barService.AddBarAsync(barModel.Name, barModel.Address, barModel.Description, barModel.Country, barModel.City, barPhoto);

            }
            catch
            {
                return View("AddBar");
            }
            return RedirectToAction("BarSearch", "Catalog");
        }
    }
}
