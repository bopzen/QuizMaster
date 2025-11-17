namespace QuizMaster.Models
{
    public class QuizScoreRecord
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public double Points { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalSeconds { get; set; }
        public DateTime DateAchieved { get; set; }
    }
}
