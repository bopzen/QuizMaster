using QuizMaster.Models;

namespace QuizMaster.Contracts
{
    public interface IQuizMasterApiService
    {
        Task<List<QuestionModel>> GetQuizQuestions(int amount, int? category, string? difficulty = null, string? type = null);
    }
}
