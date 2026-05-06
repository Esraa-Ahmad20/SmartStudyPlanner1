using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class Resource
    {
        [Key] public int ResourceId { get; set; }
        public string? Title { get; set; }
        public string? Link { get; set; }
        public int SubjectId { get; set; }
        public Subject? Subject { get; set; }
    }
}