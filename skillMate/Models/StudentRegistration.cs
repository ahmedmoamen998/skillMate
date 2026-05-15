namespace SkillMate.Models
{
    public class StudentRegistration
    {
        public int Id { get; set; }

        public string StudentId { get; set; } = string.Empty;

        public ApplicationUser? Student { get; set; }

        public int SkillClassId { get; set; }

        public SkillClass? SkillClass { get; set; }

        public DateTime JoinDate { get; set; } = DateTime.Now;
    }
}