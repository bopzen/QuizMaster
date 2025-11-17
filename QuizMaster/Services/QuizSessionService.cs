using QuizMaster.Contracts;
using QuizMaster.Models;
namespace QuizMaster.Services
{
    public class QuizSessionService : IQuizSessionService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuizSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public void ClearSession()
        {
            Session.Clear();
        }

        public void SaveQuestionsToSession(List<QuestionModel> questions)
        {
            var questionsJson = System.Text.Json.JsonSerializer.Serialize(questions);
            Session.SetString("QuizQuestions", questionsJson);
        }

        public List<QuestionModel> GetQuestionsFromSession()
        {
            var questionsJson = Session.GetString("QuizQuestions");
            if (string.IsNullOrEmpty(questionsJson))
                return new List<QuestionModel>();
            return System.Text.Json.JsonSerializer.Deserialize<List<QuestionModel>>(questionsJson) ?? new List<QuestionModel>();
        }

        public List<int> GetQuestionIdsFromSession()
        {
            var questions = GetQuestionsFromSession();
            return questions.Select(q => q.Id).ToList();
        }

        public int CountQuestionsInSession()
        {
            var questions = GetQuestionsFromSession();
            return questions?.Count ?? 0;
        }

        public QuestionModel? GetQuestionFromSessionById(int id)
        {
            var questions = GetQuestionsFromSession();
            return questions.FirstOrDefault(q => q.Id == id);
        }

        public void SaveAnswerToSession(int id, string selectedAnswer)
        {
            Session.SetString($"Answer_{id}", System.Net.WebUtility.HtmlDecode(selectedAnswer));
        }

        public string? GetAnswerFromSession(int id)
        {
            if (Session.GetString($"Answer_{id}") == null)
            {
                return "Not answered";
            }
            return Session.GetString($"Answer_{id}");
        }

        public void SaveNameToSession(string name)
        {
            Session.SetString("Name", name);
        }

        public string? GetNameFromSession()
        {
            return Session.GetString("Name");
        }

    }
}
