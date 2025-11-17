using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts;
using QuizMaster.ViewModels;

namespace QuizMaster.Controllers
{
    public class LeaderBoardController : Controller
    {
        private readonly IQuizResultService _quizResultService;

        public LeaderBoardController(IQuizResultService quizResultService)
        {
            _quizResultService = quizResultService;
        }

        public IActionResult Index()
        {
            var scores = _quizResultService.GetLeaderBoardScores();
            var model = scores.Select(s => new QuizLeaderBoardViewModel
            {
                Rank = scores.IndexOf(s) + 1,
                Name = s.Name,
                Points = s.Points,
                CorrectAnswers = $"{s.Score} / {s.TotalQuestions}",
                Time = s.TotalSeconds / 60 + " mins " + s.TotalSeconds % 60 + " seconds",
                DateAchieved = s.DateAchieved.ToString("yyyy-MM-dd HH:mm")
            }).ToList();

            return View(model);
        }
    }
}
