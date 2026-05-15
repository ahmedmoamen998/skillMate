using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMate.Data;
using SkillMate.Models;

namespace SkillMate.Controllers
{
    [Authorize]
    public class InstructorRequestsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public InstructorRequestsController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var requests = await _context.InstructorRequests
                .Include(r => r.Student)
                .OrderByDescending(r => r.SubmittedDate)
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int id)
        {
            var request = await _context.InstructorRequests
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InstructorRequest request)
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            request.StudentId = currentUser.Id;
            request.Status = "Pending";
            request.SubmittedDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.InstructorRequests.Add(request);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MyRequests));
            }

            return View(request);
        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> MyRequests()
        {
            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var requests = await _context.InstructorRequests
                .Where(r => r.StudentId == currentUser.Id)
                .OrderByDescending(r => r.SubmittedDate)
                .ToListAsync();

            return View(requests);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Approve(int id)
        {
            var request = await _context.InstructorRequests
                .Include(r => r.Student)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.Status == "Pending")
            {
                request.Status = "Approved";

                var skillClass = new SkillClass
                {
                    SkillName = request.SkillName,
                    Category = request.Category,
                    InstructorName = request.Student?.FullName ?? "Unknown Instructor",
                    Level = request.Level,
                    AvailableTime = request.AvailableTime,
                    Description = request.Description,
                    ContactInfo = request.ContactInfo,
                    MaxStudents = request.MaxStudents
                };

                _context.SkillClasses.Add(skillClass);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Reject(int id)
        {
            var request = await _context.InstructorRequests.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            if (request.Status == "Pending")
            {
                request.Status = "Rejected";
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}