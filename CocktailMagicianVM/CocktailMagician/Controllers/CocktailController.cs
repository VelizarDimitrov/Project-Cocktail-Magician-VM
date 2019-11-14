using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class CocktailController : Controller
    {
        private readonly ICocktailService cocktailService;

        public CocktailController(ICocktailService cocktailService)
        {
            this.cocktailService = cocktailService;
        }

        public IActionResult AddCocktail()
        {

            return View("AddCocktail");
        }
        public async Task<IActionResult> GetAllIngredients()
        {
            var ingredients = (await cocktailService.GetAllIngredientNamesAsync()).ToArray();
            return Json(ingredients);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCocktail()
        {
            var files = Request.Form.Files;
           
            byte[] cocktailPhoto;
            using (var stream = new MemoryStream())
            {
                await formData.CopyToAsync(stream);
                cocktailPhoto = stream.ToArray();

            }
            try
            {
                await cocktailService.AddCocktailAsync(name,primaryIngredients,ingredients,description,cocktailPhoto);

            }
            catch
            {
                return View("AddCocktail");
            }
            return RedirectToAction("CocktailSearch", "Catalog");
        }
    }
}