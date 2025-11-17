namespace QuizMaster.ViewModels
{
    public class QuizResultItem
    {
        public string Question { get; set; }
        public string Difficulty { get; set; }
        public string UserAnswer { get; set; }
        public string CorrectAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public int Points { get; set; }
    }
}
