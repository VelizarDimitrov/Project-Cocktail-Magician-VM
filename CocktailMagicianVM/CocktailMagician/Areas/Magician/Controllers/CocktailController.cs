using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
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

            return View("AddCocktail");
        }

        [HttpPost]
        public async Task<IActionResult> CreateCocktail(string name, string primaryIngredients, string ingredients, string description)
        {
            byte[] cocktailPhoto;
            var file = Request.Form.Files[0];
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                cocktailPhoto = stream.ToArray();
            }
            var primaryIngredientsArr = primaryIngredients.Split(',');
            var ingredientsArr = ingredients.Split(',');
            await cocktailService.CreateCocktailAsync(name, description, primaryIngredientsArr, ingredientsArr, cocktailPhoto);
            return Ok();
        }

        public IActionResult Manage()
        {
            return View("Cocktails");
        }

        public async Task<IActionResult> CocktailSearchResults(string keyword, string page, string rating, string pageSize)
        {
            Tuple<IList<Cocktail>, bool> cocktails;
            var model = new CocktailSearchViewModel()
            {
                Keyword=keyword==null?"":keyword,
                Page = int.Parse(page)
            };
            cocktails = await cocktailService.FindCocktailsForManagingAsync(model.Keyword, model.Page, int.Parse(pageSize));

            foreach (var cocktail in cocktails.Item1)
            {
                model.Cocktails.Add(new CocktailViewModel(cocktail));
            }
            model.LastPage = cocktails.Item2;
            return PartialView("_MagicianCocktailsView", model);
        }

        public async Task<IActionResult> HideCocktail(string cocktailId)
        {
            var id = int.Parse(cocktailId);
            await cocktailService.HideCocktailAsync(id);
            return RedirectToAction("Manage");
        }



    }
}