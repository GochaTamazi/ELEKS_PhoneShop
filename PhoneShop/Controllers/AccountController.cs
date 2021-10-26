using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using Application.DTO.Frontend;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace PhoneShop.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public AccountController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [AllowAnonymous]
        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(
            [FromForm] LoginModel loginModel,
            CancellationToken token
        )
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetOneAsync((user) =>
                        user.Email == loginModel.Email &&
                        user.Password == loginModel.Password,
                    token);
                if (user != null)
                {
                    await AuthenticateAsync(user.Email, user.Role);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View(loginModel);
        }

        [AllowAnonymous]
        [HttpGet("register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(
            [FromForm] RegisterModel loginModel,
            CancellationToken token
        )
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetOneAsync((user) => user.Email == loginModel.Email
                    , token);
                if (user == null)
                {
                    var userModel = new User
                    {
                        Email = loginModel.Email,
                        Password = loginModel.Password,
                        Role = "Customer"
                    };
                    await _usersRepository.InsertAsync(userModel, token);

                    await AuthenticateAsync(userModel.Email, userModel.Role);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect login or password");
                }
            }

            return View(loginModel);
        }

        [Authorize]
        [HttpGet("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        private async Task AuthenticateAsync(string userName, string userRole)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, userRole)
            };
            var id = new ClaimsIdentity(
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
