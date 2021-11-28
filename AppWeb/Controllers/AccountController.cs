using Application.DTO.Frontend.Forms;
using DataAccess.Interfaces;
using Database.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Threading;

namespace PhoneShop.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly IGeneralRepository<User> _usersRepository;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountController(
            IGeneralRepository<User> usersRepository,
            IPasswordHasher<User> passwordHasher
        )
        {
            _usersRepository = usersRepository;
            _passwordHasher = passwordHasher;
        }

        [AllowAnonymous]
        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(CancellationToken token, [FromForm] LoginForm loginForm)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetOneAsync(user => user.Email == loginForm.Email, token);
                if (user != null)
                {
                    var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user,
                        user.Password,
                        loginForm.Password);

                    if (passwordVerificationResult == PasswordVerificationResult.Success)
                    {
                        await AuthenticateAsync(user.Email, user.Role);
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View(loginForm);
        }

        [AllowAnonymous]
        [HttpGet("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(CancellationToken token, [FromForm] RegisterForm registerForm)
        {
            if (ModelState.IsValid)
            {
                var user = await _usersRepository.GetOneAsync(user => user.Email == registerForm.Email, token);
                if (user == null)
                {
                    var userNew = new User
                    {
                        Email = registerForm.Email,
                        Password = registerForm.Password,
                        Role = "Customer",
                        Name = registerForm.Name
                    };
                    userNew.Password = _passwordHasher.HashPassword(userNew, registerForm.Password);

                    await _usersRepository.AddAsync(userNew, token);

                    await AuthenticateAsync(userNew.Email, userNew.Role);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Incorrect login or password");
            }

            return View(registerForm);
        }

        [Authorize]
        [HttpGet("Logout")]
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