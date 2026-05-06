using System.ComponentModel.DataAnnotations;

namespace SmartStudyPlanner1.Models
{
    public class MotivationQuote
    {
        [Key] public int QuoteId { get; set; }
        public string? QuoteText { get; set; }
        public string? Category { get; set; }
    }
}