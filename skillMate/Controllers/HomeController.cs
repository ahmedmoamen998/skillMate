using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMate.Data;
using SkillMate.Models;

namespace SkillMate.Controllers
{
    //must login data anotation
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return RedirectToAction("AdminDashboard");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminDashboard()
        {
            ViewBag.TotalSkills = await _context.SkillClasses.CountAsync();
            ViewBag.PendingRequests = await _context.InstructorRequests
                .Where(r => r.Status == "Pending")
                .CountAsync();
            ViewBag.TotalRegisteredStudents = await _context.StudentRegistrations.CountAsync();

            return View();
        }
    }
}