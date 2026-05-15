using Microsoft.AspNetCore.Identity;

namespace SkillMate.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;

        public List<StudentRegistration> StudentRegistrations { get; set; } = new List<StudentRegistration>();

        public List<InstructorRequest> InstructorRequests { get; set; } = new List<InstructorRequest>();
    }
}