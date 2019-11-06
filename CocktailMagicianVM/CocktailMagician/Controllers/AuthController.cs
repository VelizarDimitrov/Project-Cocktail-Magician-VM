﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CocktailMagician.Models;
using Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Contracts;

namespace CocktailMagician.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAccountService aService;

        public AuthController(IAccountService aService)
        {
            this.aService = aService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (!this.ModelState.IsValid)
                return BackToHome();

            try
            {
                var user = await this.aService.FindUserWebAsync(vm.UserName, vm.Password);

                SignInUser(user);

              return BackToHome();
            }
            catch (Exception ex)
            {
                return BackToHome();
            }

        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View("RegisterView");
        }
        [HttpPost]
        //public async Task<IActionResult> Register(UserModel vm)
        //{
        //    if (!this.ModelState.IsValid)
        //        return BackToHome();
        //    try
        //    {
        //        vm.AccountType = "Member";
        //        await this.aService.AddAccountAsync(vm.UserName, vm.FirstName, vm.LastName, vm.Password, vm.AccountType);
        //        var user = await aService.FindUserWebAsync(vm.UserName, vm.Password);
        //        SignInUser(user);

        //        return BackToHome();
        //    }
        //    catch (Exception)
        //    {
        //        return BackToHome();
        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await this.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return BackToHome();
        }

        private async void SignInUser(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Role,user.AccountType)

            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),
                IssuedUtc = DateTimeOffset.UtcNow,
                IsPersistent = true,
            };

            await this.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        private RedirectToActionResult BackToHome()
          => RedirectToAction("Index", "Home");

    }
}