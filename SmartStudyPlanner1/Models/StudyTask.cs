using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class StudyTask
    {
        [Key] public int TaskId { get; set; }
        public int PlanId { get; set; }
        public int ChapterId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public bool IsCompleted { get; set; } = false;
        public int Priority { get; set; } = 1;
        public StudyPlan? StudyPlan { get; set; }
        public Chapter? Chapter { get; set; }
        public ProgressRecord? ProgressRecord { get; set; }
    }
}