using System.Text.Json.Serialization;

namespace QuizMaster.Models
{
    public class QuestionModel
    {
        public int Id { get; set; } 

        [JsonPropertyName("type")]
        public string QuestionType { get; set; }

        [JsonPropertyName("difficulty")]
        public string Difficulty { get; set; }

        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("question")]
        public string QuestionText { get; set; }

        [JsonPropertyName("correct_answer")]
        public string CorrectAnswer { get; set; }

        [JsonPropertyName("incorrect_answers")]
        public List<string> IncorrectAnswers { get; set; }

        public List<AnswerOptionModel> AnswerOptions { get; set; }

    }
}
