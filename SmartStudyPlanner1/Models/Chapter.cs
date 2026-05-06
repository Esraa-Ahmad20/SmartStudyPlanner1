using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class Chapter
    {
        [Key] public int ChapterId { get; set; }
        public string? ChapterTitle { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
        public ICollection<StudyTask>? StudyTasks { get; set; }
    }
}