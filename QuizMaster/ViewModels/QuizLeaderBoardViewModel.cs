using System.ComponentModel;

namespace QuizMaster.ViewModels
{
    public class QuizLeaderBoardViewModel
    {
        public int Rank { get; set; }
        public string Name { get; set; }
        public double Points { get; set; }
        [DisplayName("Correct Answers")]
        public string CorrectAnswers { get; set; }

        [DisplayName("Time Spent")]
        public string Time { get; set; }
        [DisplayName("Date Achieved")]
        public string DateAchieved { get; set; }
    }
}
