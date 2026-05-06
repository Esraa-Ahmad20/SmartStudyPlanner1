using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class ProgressRecord
    {
        [Key] public int ProgressId { get; set; }
        public int TaskId { get; set; }
        public DateTime ActualCompletionDate { get; set; } = DateTime.Now;
        public decimal? TimeSpent { get; set; }
        public string? Notes { get; set; }
        public StudyTask? StudyTask { get; set; }
    }
}