using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class Subject
    {
        [Key] public int SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public int DifficultyLevel { get; set; }
        public DateTime ExamDate { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Chapter>? Chapters { get; set; }
        public ICollection<Resource>? Resources { get; set; }
        public ICollection<PomodoroSession>? PomodoroSessions { get; set; }
    }
}