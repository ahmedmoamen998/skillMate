using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SkillMate.Models;

namespace SkillMate.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<SkillClass> SkillClasses { get; set; }

        public DbSet<StudentRegistration> StudentRegistrations { get; set; }

        public DbSet<InstructorRequest> InstructorRequests { get; set; }
    }
}