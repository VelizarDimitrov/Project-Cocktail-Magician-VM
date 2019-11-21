using Microsoft.AspNetCore.Mvc;

namespace CocktailMagician.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult PageNotFound()
        {
            return this.View();
        }
    }
}