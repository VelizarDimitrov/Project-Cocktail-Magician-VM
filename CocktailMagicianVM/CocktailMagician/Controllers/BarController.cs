using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Areas.Magician.Models;
using CocktailMagician.Models;
using Data.Models;
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

        public BarController(ICountryService countryService, ICityService cityService, IBarService barService, IAccountService aService)
        {
            this.countryService = countryService;
            this.cityService = cityService;
            this.barService = barService;
            this.aService = aService;
        }


        public async Task<IActionResult> GetAllCountries()
        {
            var countries = (await countryService.GetAllCountryNamesAsync()).ToArray();
            return Json(countries);
        }

        public async Task<IActionResult> GetAllCities(string cityName)
        {
            string[] cities;
            if (await countryService.CheckIfCountryExistsAsync(cityName))
                cities = (await cityService.GetCitiesFromCountryAsync(cityName)).ToArray();
            else
                cities = new string[0];
            return Json(cities);
        }

        [HttpPost]
        public async Task<IActionResult> BarDetails(string barId)
        {
            var bar = await barService.FindBarByIdAsync(int.Parse(barId));
            var vm = new BarViewModel(bar);
            return View("BarDetails", vm);
        }

        public async Task<IActionResult> LoadBarComments(string barId, string commentCount)
        {
            int loadNumber = int.Parse(commentCount);
            var comments = (await barService.GetBarCommentsAsync(int.Parse(barId), loadNumber));
            var vm = new BarCommentListViewModel(comments);
            return PartialView("_BarCommentsView", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddBarComment(BarViewModel vm)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.AddBarCommentAsync(vm.Id, vm.CreateComment, userId);
            var bar = await barService.FindBarByIdAsync(vm.Id);
            var barForView = new BarViewModel(bar);
            return View("BarDetails", barForView);
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

        public async Task<IActionResult> BarSearchResults(string keyword, string criteria, string order, string page, string rating, string sortOrder, string pageSize)
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
            var rate = rating == null ? "1;5" : rating;

            searchResults = await barService.FindBarsForCatalogAsync(model.Keyword, model.SelectedCriteria, model.Page, model.SelectedOrderBy, rate, sortOrder, int.Parse(pageSize));

            foreach (var bar in searchResults.Item1)
            {
                model.Bars.Add(new BarViewModel(bar));
            }
            model.LastPage = searchResults.Item2;
            return PartialView("_BarSearchView", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetBarPhoto(int id)
        {
            var picture = await barService.FindBarPhotoAsync(id);

            return File(picture, "image/png");
        }

        public async Task<IActionResult> GetAllBars(string cityName)
        {
            string[] bars;
            if (await cityService.CheckIfCityExistsAsync(cityName))
                bars = (await barService.GetBarsFromCityAsync(cityName)).ToArray();
            else
                bars = (await barService.GetAllBarNamesAsync()).ToArray();
            return Json(bars);
        }

        public async Task<IActionResult> GoToBar(IndexViewModel input)
        {
            var bar = await barService.FindBarByNameAsync(input.BarName);
            if (bar != null)
            {
                var vm = new BarViewModel(bar);
                return View("BarDetails", vm);
            }
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> UnFavoriteBar(string barId)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.RemoveBarFromFavoritesAsync(int.Parse(barId),userId);
            return Ok();
        }
    }
}
