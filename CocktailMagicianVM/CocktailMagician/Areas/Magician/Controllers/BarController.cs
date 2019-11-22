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
        private readonly ICocktailService cService;

        public BarController(ICountryService countryService, ICityService cityService, IBarService barService, IAccountService aService, ICocktailService cService)
        {
            this.countryService = countryService;
            this.cityService = cityService;
            this.barService = barService;
            this.aService = aService;
            this.cService = cService;
        }

        public IActionResult AddBar()
        {
            var vm = new AddBarViewModel();
            return View("AddBar", vm);
        }

        [HttpPost]
        //[AutoValidateAntiforgeryToken]
        public async Task<IActionResult> AddBar(AddBarViewModel barModel, IFormFile file)
        {
            if (file != null && this.ModelState.IsValid)
            {
                byte[] barPhoto;
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    barPhoto = stream.ToArray();
                }
                await barService.AddBarAsync(barModel.Name, barModel.Address, barModel.Description, barModel.Country, barModel.City, barPhoto);
            }
            else
            {

                return View("AddBar", barModel);
            }

            return RedirectToAction("BarSearch", "Bar");
        }

        public IActionResult Manage()
        {
            return View("Bars");
        }

        [HttpPost]
        public async Task<IActionResult> BarSearchResults(string keyword, string page, string pageSize)
        {
            Tuple<IList<Bar>, bool> bars;
            var model = new BarSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            bars = await barService.FindBarsForUserAsync(model.Keyword, model.Page, int.Parse(pageSize), null);

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

        public IActionResult EditBarCocktails(string barId)
        {
            var vm = new ManageBarCocktailsViewModel(int.Parse(barId));
            return View("BarCocktails", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddCocktail(string cocktailId, string barId)
        {
            var bId = int.Parse(barId);
            var cId = int.Parse(cocktailId);
            await barService.AddCocktailBarAsync(bId, cId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCocktail(string cocktailId, string barId)
        {
            var bId = int.Parse(barId);
            var cId = int.Parse(cocktailId);
            await barService.RemoveCoctailBarAsync(bId, cId);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> EditBar(string barId)
        {
            var id = int.Parse(barId);
            var bar = await barService.FindBarByIdAsync(id);
            var vm = new EditBarViewModel(bar);
            return View("EditBar", vm);
        }

        [HttpPost]
        public async Task<IActionResult> EditBar(EditBarViewModel barModel, IFormFile file)
        {
            if (!this.ModelState.IsValid)
                return View("EditBar", barModel);
            byte[] barPhoto = null;
            if (file != null && this.ModelState.IsValid)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    barPhoto = stream.ToArray();
                }
            }

            await barService.UpdateBarAsync(barModel.Id ,barModel.Name, barModel.Address, barModel.Description, barModel.Country, barModel.City, barPhoto);
            return RedirectToAction("Manage");
        }

    }
}