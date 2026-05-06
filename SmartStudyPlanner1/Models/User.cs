using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class User
    {
        [Key] public int UserId { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public ICollection<Subject>? Subjects { get; set; }
        public ICollection<StudyPlan>? StudyPlans { get; set; }
    }
}