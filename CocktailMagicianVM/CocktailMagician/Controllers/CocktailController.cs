using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CocktailMagician.Controllers
{
    public class CocktailController : Controller
    {
        public IActionResult AddCocktail()
        {

            return View("AddCocktail");
        }
    }
}