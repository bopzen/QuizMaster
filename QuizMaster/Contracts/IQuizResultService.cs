using QuizMaster.Models;
using QuizMaster.ViewModels;

namespace QuizMaster.Contracts
{
    public interface IQuizResultService
    {
        string GenerateResultsComment(double percentage);
        List<QuizResultItem> EvaluateAnswers(List<QuestionModel> questions);
        double CalculatePercentage(int score, int totalQuestions);
        double CalculateTotalPoints(List<QuizResultItem> results, int totalSeconds, int allowedSeconds);
        void SaveScoreToJsonFile(string userName, int score, double points, int totalQuestions, int totalSeconds);
        string CheckQualification(double points);
        List<QuizScoreRecord> GetLeaderBoardScores();
    }
}
