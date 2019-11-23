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
    public class CocktailController : Controller
    {
        private readonly ICocktailService cocktailService;
        private readonly IIngredientService iService;
        private readonly IAccountService aService;

        public CocktailController(ICocktailService cocktailService, IIngredientService iService, IAccountService aService)
        {
            this.cocktailService = cocktailService;
            this.iService = iService;
            this.aService = aService;
        }

        public IActionResult AddCocktail()
        {
            var vm = new AddCocktailViewModel();
            return View("AddCocktail", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddCocktail(AddCocktailViewModel model, IFormFile file)
        {
            if (!String.IsNullOrWhiteSpace(model.MainIngString))
                model.MainIngredients = model.MainIngString.Split(',').ToList();
            else
                return View("AddCocktail", model);

            if (!this.ModelState.IsValid || model.MainIngredients.All(p => String.IsNullOrWhiteSpace(p)) || file == null)
            {
                return View("AddCocktail", model);
            }

            byte[] cocktailPhoto;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                cocktailPhoto = stream.ToArray();
            }
            await cocktailService.CreateCocktailAsync(model.Name, model.Description, model.MainIngredients.ToArray(), model.IngString == null ? null : model.IngString.Split(','), cocktailPhoto);

            return RedirectToAction("Manage");
        }

        public IActionResult Manage()
        {
            return View("Cocktails");
        }

        public async Task<IActionResult> CocktailSearchResults(string keyword, string page, string pageSize)
        {
            Tuple<IList<Cocktail>, bool> cocktails;
            var model = new CocktailSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            cocktails = await cocktailService.FindCocktailsForUserAsync(model.Keyword, model.Page, int.Parse(pageSize), null);

            foreach (var cocktail in cocktails.Item1)
            {
                model.Cocktails.Add(new CocktailViewModel(cocktail));
            }
            model.LastPage = cocktails.Item2;
            return PartialView("_MagicianCocktailsView", model);
        }
        public async Task<IActionResult> CocktailManageResults(string keyword, string page, string pageSize, string barId)
        {
            Tuple<IList<Cocktail>, bool> cocktails;
            var model = new CocktailSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page),
                BarId = int.Parse(barId)
            };
            cocktails = await cocktailService.FindCocktailsForUserAsync(model.Keyword, model.Page, int.Parse(pageSize), null);

            foreach (var cocktail in cocktails.Item1)
            {
                model.Cocktails.Add(new CocktailViewModel(cocktail));
            }
            model.LastPage = cocktails.Item2;
            return PartialView("_BarCocktails", model);
        }

        [HttpPost]
        public async Task<IActionResult> HideCocktail(string cocktailId)
        {
            var id = int.Parse(cocktailId);
            await cocktailService.HideCocktailAsync(id);
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public async Task<IActionResult> UnhideCocktail(string cocktailId)
        {
            var id = int.Parse(cocktailId);
            await cocktailService.UnhideCocktailAsync(id);
            return RedirectToAction("Manage");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string cocktailId)
        {
            var id = int.Parse(cocktailId);
            var cocktail = await cocktailService.FindCocktailByIdAsync(id);
            var ingredients = await iService.GetIngredientsByCocktailAsync(id);
            var vm = new EditCocktailViewModel(cocktail, ingredients);
            return View("EditCocktail", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditCocktailViewModel model, IFormFile file)
        {
            if (!String.IsNullOrWhiteSpace(model.MainIngString))
                model.MainIngredients = model.MainIngString.Split(',').ToList();
            else
                return View("EditCocktail", model);

            if (!this.ModelState.IsValid || model.MainIngredients.All(p => String.IsNullOrWhiteSpace(p)))
            {
                return View("EditCocktail", model);
            }

            byte[] cocktailPhoto = null;
            if (file != null)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    cocktailPhoto = stream.ToArray();
                }
            }
            await cocktailService.UpdateCocktailAsync(model.Id, model.Name, model.Description, model.MainIngredients.ToArray(), model.IngString == null ? null : model.IngString.Split(','), cocktailPhoto);

            return RedirectToAction("Manage");

        }

    }
}