using System;
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
                await aService.SetLastLoginAsync(user.Id);
                return BackToHome();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login");
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

        public async Task<IActionResult> Register(LoginViewModel vm)
        {
            if (!this.ModelState.IsValid)
                return BackToHome();
            try
            {
                await this.aService.AddAccountAsync(vm.UserName, vm.FirstName, vm.LastName, vm.Password, "Bar Crawler", vm.Country, vm.City);
                var user = await aService.FindUserWebAsync(vm.UserName, vm.Password);
                SignInUser(user);

                return BackToHome();
            }
            catch (Exception)
            {
                return BackToHome();
            }
        }

        [HttpGet]
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

        public async Task<IActionResult> CheckIfUsernameAvailable(string username)
        {
            var user = await aService.FindUserByUserNameAsync(username);
            if (user == null)
            {
                return Json("available");
            }
            else
            {
                return Json("unavailable");
            }
        }
        public async Task<IActionResult> CheckIfPasswordIsCorrect(string password)
        {
            var userId = int.Parse(this.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);
            var validate = await aService.ValidateUserPasswordAsync(userId,password);
            if (validate)
            {
                return Json("correct");
            }
            else
            {
                return Json("incorrect");
            }
        }
    }
}