using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.Dtos;
using ECommerce.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace ECommerce.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, 
                isPersistent: true, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index), "Home");
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View();
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpDto signUpDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            ApplicationUser user = new()
            {
                Name = signUpDto.Name,
                Email = signUpDto.Email,
                UserName = signUpDto.Email,
                PhoneNumber = signUpDto.PhoneNumber
            };

            IdentityResult result = await _userManager.CreateAsync(user, signUpDto.Password);

            if (result.Succeeded)
            {
                await _signInManager.PasswordSignInAsync(user, signUpDto.Password, 
                    isPersistent: true, lockoutOnFailure: false);
                await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach(var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> IsEmailNotInUse(string email)
        {
            var isNotInUse = await _userManager.FindByEmailAsync(email) is null;

            return Ok(isNotInUse);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
