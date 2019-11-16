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
        private readonly IAccountService aService;

        public BarController(ICountryService countryService, ICityService cityService, IBarService barService,IAccountService aService)
        {
            this.countryService = countryService;
            this.cityService = cityService;
            this.barService = barService;
            this.aService = aService;
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
            if (await countryService.CheckIfCountryExists(cityName))
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
        [HttpPost]
        public async Task<IActionResult> BarDetails(string barId)
        {     
            var bar = await barService.FindBarByIdAsync(int.Parse(barId));
            var vm= new BarViewModel(bar);
            return View("BarDetails", vm);

        }
        public async Task<IActionResult> LoadBarComments(string barId,string commentCount)
        {
            int loadNumber = int.Parse(commentCount);
            var comments = (await barService.GetBarCommentsAsync(int.Parse(barId),loadNumber));
            var vm = new BarCommentListViewModel(comments);
            return PartialView("_BarCommentsView",vm);
        }
        [HttpPost]
        public async Task<IActionResult> AddBarComment(BarViewModel vm)
        {
            var userId= int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.AddBarCommentAsync(vm.Id, vm.CreateComment, userId);
            var bar = await barService.FindBarByIdAsync(vm.Id);
            var barForView = new BarViewModel(bar);
            return View("BarDetails", barForView);
        }
    }
}
