namespace QuizMaster.ViewModels
{
    public class QuizResultViewModel
    {
        public string Name { get; set; }
        public List<QuizResultItem> Results { get; set; } = new();
        public int Score { get; set; }
        public double Points { get; set; }
        public double Percentage { get; set; }
        public string Comment { get; set; }
        public int TotalQuestions { get; set; }
        public int TotalSeconds { get; set; }
    }
}
