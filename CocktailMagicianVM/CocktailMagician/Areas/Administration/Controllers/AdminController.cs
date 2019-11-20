using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Areas.Administration.Models;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountService aService;

        public AdminController(IAccountService aService)
        {
            this.aService = aService;
        }

        public IActionResult ManageUsers()
        {
            return View("Users");
        }
        public async Task<IActionResult> UserSearchResults(string keyword, string page, string pageSize)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            Tuple<IList<User>, bool> users;
            var model = new UserSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            users = await aService.FindUsersForAdminAsync(model.Keyword, model.Page, int.Parse(pageSize));
           var admin = users.Item1.Where(p => p.Id == userId).FirstOrDefault();
            users.Item1.Remove(admin);
            foreach (var user in users.Item1)
            {
                model.Users.Add(new UserViewModel(user));
            }
            model.LastPage = users.Item2;
            return PartialView("_AdminUsersResultView", model);
        }
        [HttpPost]
        public async Task<IActionResult> UnFreezeUser(string userId)
        {
            await aService.UnFreezeUserAsync(int.Parse(userId));
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> FreezeUser(string userId)
        {
            await aService.FreezeUserAsync(int.Parse(userId));
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> PromoteUser(string userId)
        {
            await aService.PromoteUserAsync(int.Parse(userId));
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> DemoteUser(string userId)
        {
            await aService.DemoteUserAsync(int.Parse(userId));
            return Ok();
        }
    }
}