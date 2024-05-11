using ECommerce.Core.Domain.IdentityEntities;
using ECommerce.Core.Dtos;
using ECommerce.Core.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.UI.Controllers
{
    /// <summary>
    /// Controller responsible for handling account-related actions such as login, signup, and logout.
    /// </summary>
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="userManager">The user manager service.</param>
        /// <param name="signInManager">The sign-in manager service.</param>
        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Displays the login view.
        /// </summary>
        /// <returns>The login view.</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Handles the login process.
        /// </summary>
        /// <param name="loginDto">The login DTO containing user credentials.</param>
        /// <returns>A redirect to the home page if login is successful, 
        /// otherwise the login view with an error message.</returns>
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
                var user = await _userManager.FindByEmailAsync(loginDto.Email);
                var userRoles = await _userManager.GetRolesAsync(user!);

                return RedirectToAction(nameof(Index), "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }

        /// <summary>
        /// Displays the sign-up view.
        /// </summary>
        /// <returns>The sign-up view.</returns>
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        /// <summary>
        /// Handles the sign-up process.
        /// </summary>
        /// <param name="signUpDto">The sign-up DTO containing user details.</param>
        /// <returns>A redirect to the home page if sign-up is successful, 
        /// otherwise the sign-up view with error messages.</returns>
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
                await _userManager.AddToRoleAsync(user, Constants.ROLE_CUSTOMER);
                await _signInManager.RefreshSignInAsync(user);

                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        /// <summary>
        /// Checks if an email is not in use.
        /// </summary>
        /// <param name="email">The email to check.</param>
        /// <returns>A JSON result indicating whether the email is not in use.</returns>
        [HttpGet]
        public async Task<IActionResult> IsEmailNotInUse(string email)
        {
            var isNotInUse = await _userManager.FindByEmailAsync(email) is null;

            return Ok(isNotInUse);
        }

        /// <summary>
        /// Logs the user out.
        /// </summary>
        /// <returns>A redirect to the login page.</returns>
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction(nameof(Login));
        }
    }
}
