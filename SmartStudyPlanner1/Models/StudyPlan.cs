using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class StudyPlan
    {
        [Key] public int PlanId { get; set; }
        public string? PlanName { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int DailyStudyHours { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<StudyTask>? StudyTasks { get; set; }
    }
}