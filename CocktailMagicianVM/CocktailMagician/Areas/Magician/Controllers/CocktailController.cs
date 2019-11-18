using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
    }
}