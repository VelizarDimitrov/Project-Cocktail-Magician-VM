using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService aService;

        public AccountController(IAccountService aService)
        {
            this.aService = aService;
        }

        [HttpPost]
        public async Task<IActionResult> RateBar(string userRating, string id)
        {
           var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
           await aService.RateBarAsync(userId, int.Parse(userRating), int.Parse(id));
            return Ok();
        }
    }
}