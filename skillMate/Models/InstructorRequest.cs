namespace SkillMate.Models
{
    public class InstructorRequest
    {
        public int Id { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public ApplicationUser? Student { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        public string AvailableTime { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ContactInfo { get; set; } = string.Empty;

        public int MaxStudents { get; set; }

        public string Status { get; set; } = "Pending";

        public DateTime SubmittedDate { get; set; } = DateTime.Now;
    }
}