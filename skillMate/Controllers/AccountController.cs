using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillMate.Models;

namespace SkillMate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string password,
            string confirmPassword)
        {
            ViewData["FullName"] = fullName;
            ViewData["Email"] = email;

            if (string.IsNullOrWhiteSpace(fullName))
            {
                ModelState.AddModelError("fullName", "Full name is required.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("email", "Email is required.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("password", "Password is required.");
            }

            if (password != confirmPassword)
            {
                ModelState.AddModelError("confirmPassword", "Password and Confirm Password do not match.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new ApplicationUser
            {
                FullName = fullName,
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "User");

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string password,
            bool rememberMe)
        {
            ViewData["Email"] = email;
            ViewData["RememberMe"] = rememberMe;

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError("email", "Email is required.");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError("password", "Password is required.");
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                rememberMe,
                lockoutOnFailure: false
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid email or password.");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}
