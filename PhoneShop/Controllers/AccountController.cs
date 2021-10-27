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
using Microsoft.AspNetCore.Identity;

namespace PhoneShop.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(
            IUsersRepository usersRepository,
            IPasswordHasher<User> passwordHasher
        )
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
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
                var user = await _usersRepository.GetOneAsync((user) => user.Email == loginModel.Email, token);
                if (user != null)
                {
                    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user,
                        user.Password,
                        loginModel.Password);

                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        await AuthenticateAsync(user.Email, user.Role);
                        return RedirectToAction("Index", "Home");
                    }
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
                var user = await _usersRepository.GetOneAsync((user) => user.Email == loginModel.Email, token);
                if (user == null)
                {
                    var userNew = new User
                    {
                        Email = loginModel.Email,
                        Password = loginModel.Password,
                        Role = "Customer"
                    };
                    userNew.Password = _passwordHasher.HashPassword(userNew, loginModel.Password);

                    await _usersRepository.InsertAsync(userNew, token);

                    await AuthenticateAsync(userNew.Email, userNew.Role);
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
                new(ClaimsIdentity.DefaultNameClaimType, userName),
                new(ClaimsIdentity.DefaultRoleClaimType, userRole)
            };
            var id = new ClaimsIdentity(
                claims, "ApplicationCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}