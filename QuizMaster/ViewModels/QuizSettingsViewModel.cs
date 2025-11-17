using QuizMaster.Enums;
using System.ComponentModel.DataAnnotations;

namespace QuizMaster.ViewModels
{
    public class QuizSettingsViewModel
    {
        [Display(Name = "Number of Questions")]
        [Range(5, 30, ErrorMessage = "Please select a number between 5 and 30.")]
        public int NumberOfQuestions { get; set; }
        public EnCategory? Category { get; set; }
        public string? Difficulty { get; set; }
        public string? Type { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
