using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillMate.Data;
using SkillMate.Models;

namespace SkillMate.Controllers
{
    [Authorize]
    public class SkillClassesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SkillClassesController(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Index()
        {
            var skills = await _context.SkillClasses
                .Include(s => s.StudentRegistrations)
                .ToListAsync();

            return View(skills);
        }

        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int id)
        {
            var skillClass = await _context.SkillClasses
                .Include(s => s.StudentRegistrations)
                .ThenInclude(r => r.Student)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skillClass == null)
            {
                return NotFound();
            }

            return View(skillClass);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SkillClass skillClass)
        {
            if (ModelState.IsValid)
            {
                _context.SkillClasses.Add(skillClass);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(skillClass);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var skillClass = await _context.SkillClasses.FindAsync(id);

            if (skillClass == null)
            {
                return NotFound();
            }

            return View(skillClass);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SkillClass skillClass)
        {
            if (id != skillClass.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.SkillClasses.Update(skillClass);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(skillClass);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var skillClass = await _context.SkillClasses
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skillClass == null)
            {
                return NotFound();
            }

            return View(skillClass);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var skillClass = await _context.SkillClasses.FindAsync(id);

            if (skillClass != null)
            {
                _context.SkillClasses.Remove(skillClass);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Join(int id)
        {
            var skillClass = await _context.SkillClasses
                .Include(s => s.StudentRegistrations)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skillClass == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            if (skillClass.StudentRegistrations.Count >= skillClass.MaxStudents)
            {
                TempData["Error"] = "This class is full.";
                return RedirectToAction(nameof(Index));
            }

            bool alreadyJoined = await _context.StudentRegistrations
                .AnyAsync(r => r.SkillClassId == id && r.StudentId == currentUser.Id);

            if (alreadyJoined)
            {
                TempData["Error"] = "You already joined this class.";
                return RedirectToAction(nameof(Index));
            }

            var registration = new StudentRegistration
            {
                StudentId = currentUser.Id,
                SkillClassId = id,
                JoinDate = DateTime.Now
            };

            _context.StudentRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Success"] = "You joined successfully.";

            return RedirectToAction(nameof(Index));
        }
    }
}