using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService aService;
        private readonly IBarService barService;
        private readonly ICocktailService cocktailService;

        public AccountController(IAccountService aService,IBarService barService,ICocktailService cocktailService)
        {
            this.aService = aService;
            this.barService = barService;
            this.cocktailService = cocktailService;
        }

        [HttpPost]
        public async Task<IActionResult> RateBar(string userRating, string id)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.RateBarAsync(userId, int.Parse(userRating), int.Parse(id));
            return Ok();
        }

        public async Task<IActionResult> RateCocktail(string userRating, string id)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.RateCocktailAsync(userId, int.Parse(userRating), int.Parse(id));
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetProfilePic(int id)
        {
            var picture = await aService.FindUserAvatar(id);
            if (picture == null)
            {
                picture = System.IO.File.ReadAllBytes(@"../CocktailMagician/wwwroot/images/default-avatar.jpg");
            }
            return File(picture, "image/png");
        }

        public async Task<IActionResult> UserPage()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            vm.Initial = "userPage";
            return View("Profile", vm);
        }
        public async Task<IActionResult> UserProfilePartial()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            return PartialView("_ProfileInfoView", vm);
        }
        public async Task<IActionResult> UserDashboard(string pageSize, string page)
        {

            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            vm.Page = int.Parse(page);

            int number = (int.Parse(page) - 1) * int.Parse(pageSize);
            vm.Notifications.Notifications = vm.Notifications.Notifications.Skip(number).ToList();
            if (vm.Notifications.Notifications.Count > int.Parse(pageSize))
            {
                vm.LastPage = false;
            }
            vm.Notifications.Notifications = vm.Notifications.Notifications.Take(int.Parse(pageSize)).ToList();
            return PartialView("_NotificationsView", vm);
        }
        public async Task<IActionResult> UserProfileUpdate()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            return PartialView("_EditProfileView", vm);
        }
        [HttpPost]
        public async Task<IActionResult> UserProfileUpdate(UserViewModel userModel, IFormFile file)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            byte[] userPhoto;
            if (file != null)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    userPhoto = stream.ToArray();

                }
            }
            else userPhoto = null;
            try
            {
                await aService.UpdateProfileAsync(userId, userModel.UserName, userModel.FirstName, userModel.LastName, userPhoto);
            }
            catch
            {
                return RedirectToAction("UserPage", "Account");
            }
            return RedirectToAction("UserPage", "Account");
        }
        public async Task<IActionResult> UserPasswordUpdate()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            return PartialView("_ChangePasswordView", vm);
        }
        [HttpPost]
        public async Task<IActionResult> UserPasswordUpdate(UserViewModel vm,string newPassword)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            await aService.UpdatePasswordAsync(userId, newPassword);
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> UserFavoriteBars()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            vm.Initial = "favoriteBars";
            return View("Profile", vm);
        }
        public async Task<IActionResult> UserProfileFavoriteCocktails()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new UserViewModel(user);
            vm.Initial = "favoriteCocktails";
            return View("Profile", vm);
        }
        public IActionResult FavoriteBarsPartial()
        {
            
            return PartialView("_FavoriteBarsView");
        }
        public IActionResult FavoriteCocktailsPartial()
        {
            return PartialView("_FavoriteCocktailsView");
        }
        public async Task<IActionResult> FavoriteBarsResultPartial(string keyword,string pageSize,string page)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            Tuple<IList<Bar>, bool> bars;
            var model = new BarSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            bars = await barService.FindBarsForCatalogAsync(model.Keyword, model.Page, int.Parse(pageSize),userId);

            foreach (var bar in bars.Item1)
            {
                model.Bars.Add(new BarViewModel(bar));
            }
            model.LastPage = bars.Item2;
            return PartialView("_FavoriteBarsResultView",model);
        }
        public async Task<IActionResult> FavoriteCocktailsResultPartial(string keyword, string pageSize, string page)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            Tuple<IList<Cocktail>, bool> cocktails;
            var model = new CocktailSearchViewModel()
            {
                Keyword = keyword == null ? "" : keyword,
                Page = int.Parse(page)
            };
            cocktails = await cocktailService.FindCocktailsForCatalogAsync(model.Keyword, model.Page, int.Parse(pageSize), userId);

            foreach (var cocktail in cocktails.Item1)
            {
                model.Cocktails.Add(new CocktailViewModel(cocktail));
            }
            model.LastPage = cocktails.Item2;
            return PartialView("_FavoriteCocktailsResultView", model);
        }
        public async Task<IActionResult> FavoriteBar(string barId)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.FavoriteBarAsync(userId,int.Parse(barId));
            return Ok();
        }
        public async Task<IActionResult> FavoriteCocktail(string cocktailId)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            await aService.FavoriteCocktailAsync(userId, int.Parse(cocktailId));
            return Ok();
        }
        public async Task<IActionResult> CheckForFavoriteBar(string barId)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
          var result = await aService.CheckForFavoriteBarAsync(userId, int.Parse(barId));
            return Json(result);
        }
        public async Task<IActionResult> CheckForFavoriteCocktail(string cocktailId)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var result = await aService.CheckForFavoriteCocktailAsync(userId, int.Parse(cocktailId));
            return Json(result);
        }
    }
}