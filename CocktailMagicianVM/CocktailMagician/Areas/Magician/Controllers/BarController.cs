using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Areas.Magician.Models;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Areas.Magician.Controllers
{
    [Area("Magician")]
    [Authorize(Roles = "Cocktail Magician")]
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

        public IActionResult AddBar()
        {
            var vm = new AddBarViewModel();
            return View("AddBar", vm);
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
            return RedirectToAction("BarSearch", "Bar");
        }

        public IActionResult Manage()
        {
            return View("Bars");
        }

        public async Task<IActionResult> BarSearchResults(string keyword, string page, string pageSize)
        {
            Tuple<IList<Bar>, bool> bars;
            var model = new BarSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            bars = await barService.FindBarForCatalogAsync(model.Keyword, model.Page, int.Parse(pageSize));

            foreach (var bar in bars.Item1)
            {
                model.Bars.Add(new BarViewModel(bar));
            }
            model.LastPage = bars.Item2;
            return PartialView("_MagicianBarsView", model);
        }

        [HttpPost]
        public async Task<IActionResult> HideBar(string barId)
        {
            var id = int.Parse(barId);
            await barService.HideBarAsync(id);
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public async Task<IActionResult> UnhideBar(string barId)
        {
            var id = int.Parse(barId);
            await barService.UnhideBarAsync(id);
            return RedirectToAction("Manage");
        }
    }
}