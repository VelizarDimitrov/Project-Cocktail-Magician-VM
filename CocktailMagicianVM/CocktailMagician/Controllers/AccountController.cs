using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Microsoft.AspNetCore.Http;
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
    }
}