namespace SkillMate.Models
{
    public class SkillClass
    {
        public int Id { get; set; }

        public string SkillName { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string InstructorName { get; set; } = string.Empty;

        public string Level { get; set; } = string.Empty;

        public string AvailableTime { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ContactInfo { get; set; } = string.Empty;

        public int MaxStudents { get; set; }

        public List<StudentRegistration> StudentRegistrations { get; set; } = new List<StudentRegistration>();
    }
}