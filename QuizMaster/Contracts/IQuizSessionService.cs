using QuizMaster.Models;

namespace QuizMaster.Contracts
{
    public interface IQuizSessionService
    {
        void SaveQuestionsToSession(List<QuestionModel> questions);
        List<int> GetQuestionIdsFromSession();
        List<QuestionModel> GetQuestionsFromSession();
        QuestionModel GetQuestionFromSessionById(int questionId);
        void SaveAnswerToSession(int questionId, string answer);
        void SaveNameToSession(string name);
        string? GetNameFromSession();
        string? GetAnswerFromSession(int questionId);
        int CountQuestionsInSession();
        void ClearSession();
    }
}
