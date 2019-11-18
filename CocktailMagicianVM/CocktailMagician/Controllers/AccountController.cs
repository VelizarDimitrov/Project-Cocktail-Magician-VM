﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CocktailMagician.Models;
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
        public async Task<IActionResult> UserDashboard()
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var user = await aService.FindUserByIdAsync(userId);
            var vm = new NotificationListViewModel(user.Notifications);
            return PartialView("_NotificationsView", vm);
        }
    }
}