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
        public async Task<IActionResult> Register(
            string fullName,
            string email,
            string password,
            string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(fullName) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please fill all fields.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Password and Confirm Password do not match.";
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
                await _signInManager.SignInAsync(user, false);

                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
         
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(
            string email,
            string password,
            bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Please enter email and password.";
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(
                email,
                password,
                rememberMe,
                false
            );

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password.";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }
    }
}