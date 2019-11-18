using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
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

        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = (await iService.GetAllIngredientNamesAsync()).ToArray();
            return Json(ingredients);
        }

        [HttpGet]
        public async Task<IActionResult> CocktailSearch()
        {            var ingredients = await iService.GetAllMainIngredientsAsync();
            var vm = new CocktailCatalogViewModel(ingredients);
            return View("CocktailCatalog", vm);
        }
        public async Task<IActionResult> CocktailSearchResults(string keyword, string criteria, string order, string page, string rating, string sortOrder, string mainIngredient, string rowSize)
        {
            Tuple<IList<Cocktail>, bool> cocktails;
            var model = new CocktailSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                SelectedCriteria = criteria,
                SelectedOrderBy = order == null ? "" : order,
                Searched = true,
                Page = int.Parse(page),
                RowSize = int.Parse(rowSize)
            };
            var ing = mainIngredient == null ? "" : mainIngredient;
            var rate = rating == null ? "1;5" : rating;

            cocktails = await cocktailService.FindCocktailsForCatalogAsync(model.Keyword, model.SelectedCriteria, model.Page, model.SelectedOrderBy, rate, sortOrder, ing, 12);

            foreach (var cocktail in cocktails.Item1)
            {
                model.Cocktails.Add(new CocktailViewModel(cocktail));
            }
            model.LastPage = cocktails.Item2;
            return PartialView("_CocktailSearchView", model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCocktailPhoto(int id)
        {
            var picture = await cocktailService.FindCocktailPhotoAsync(id);
            return File(picture, "image/png");
        }

        [HttpPost]
        public async Task<IActionResult> CocktailDetails(string cocktailId)
        {
            var Cocktail = await cocktailService.FindCocktailByIdAsync(int.Parse(cocktailId));
            var vm = new CocktailViewModel(Cocktail);
            return View("CocktailDetails", vm);
        }

        public async Task<IActionResult> LoadCocktailComments(string barId, string commentCount)
        {
            int loadNumber = int.Parse(commentCount);
            var comments = (await cocktailService.GetCocktailCommentsAsync(int.Parse(barId), loadNumber));
            var vm = new CocktailCommentListViewModel(comments);
            return PartialView("_CocktailCommentsView", vm);
        }

        [HttpPost]
        public async Task<IActionResult> AddCocktailComment(CocktailViewModel vm)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.AddCocktailCommentAsync(vm.Id, vm.CreateComment, userId);
            var bar = await cocktailService.FindCocktailByIdAsync(vm.Id);
            var barForView = new CocktailViewModel(bar);
            return View("CocktailDetails", barForView);
        }

    }
}