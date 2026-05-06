using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class PomodoroSession
    {
        [Key] public int SessionId { get; set; }
        public DateTime? StartTime { get; set; }
        public int DurationMinutes { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}