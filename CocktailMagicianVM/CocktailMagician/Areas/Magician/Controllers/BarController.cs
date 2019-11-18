using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Areas.Magician.Models;
using CocktailMagician.Models;
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
    }
}