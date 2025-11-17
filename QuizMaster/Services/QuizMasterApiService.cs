using QuizMaster.Contracts;
using QuizMaster.Models;

namespace QuizMaster.Services
{
    public class QuizMasterApiService : IQuizMasterApiService
    {
        private readonly HttpClient _httpClient;
        public QuizMasterApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://opentdb.com/");
        }

        public async Task<List<QuestionModel>> GetQuizQuestions(int amount, int? category, string? difficulty = null, string? type = null)
        {
            return await GetQuizDataAsync(amount, category, difficulty, type);
        }

        private async Task<List<QuestionModel>> GetQuizDataAsync(int amount, int? category, string? difficulty = null, string? type = null)
        {
            var urlString = $"api.php?amount={amount}";
            if (category.HasValue)
            {
                urlString += $"&category={category}";
            }
            if (!string.IsNullOrEmpty(difficulty))
            {
                urlString += $"&difficulty={difficulty}";
            }
            if (!string.IsNullOrEmpty(type))
            {
                urlString += $"&type={type}";
            }

            var response = await _httpClient.GetAsync(urlString);
            response.EnsureSuccessStatusCode();

            var quizResponse = await response.Content.ReadFromJsonAsync<QuizMasterApiResponseModel>();

            var questions = quizResponse?.Results ?? new List<QuestionModel>();

            ConvertQuestionsToDomainModel(questions);

            return questions;
        }

        private List<QuestionModel> ConvertQuestionsToDomainModel(List<QuestionModel> questions)
        {
            for (int i = 0; i < questions.Count; i++) {
                var question = questions[i];
                question.Id = i + 1;
                question.QuestionText = System.Net.WebUtility.HtmlDecode(question.QuestionText);
                question.Category = System.Net.WebUtility.HtmlDecode(question.Category);
                var answerOptions = new List<AnswerOptionModel>();
                answerOptions.Add(new AnswerOptionModel
                {
                    Text = System.Net.WebUtility.HtmlDecode(question.CorrectAnswer),
                    IsCorrect = true
                });
                foreach (var incorrectAnswer in question.IncorrectAnswers)
                {
                    answerOptions.Add(new AnswerOptionModel
                    {
                        Text = System.Net.WebUtility.HtmlDecode(incorrectAnswer),
                        IsCorrect = false
                    });
                }
                question.AnswerOptions = answerOptions.OrderBy(a => Guid.NewGuid()).ToList();
            }
            return questions;
        }
    }
}
