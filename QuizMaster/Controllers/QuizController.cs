using Microsoft.AspNetCore.Mvc;
using QuizMaster.Services;
using QuizMaster.Contracts;
using QuizMaster.ViewModels;


namespace QuizMaster.Controllers
{
    
    public class QuizController : Controller
    {
        private readonly IQuizMasterApiService _quizMasterApiService;
        private readonly IQuizTimerService _quizTimer;
        private readonly IQuizSessionService _quizSessionService;
        private readonly IQuizResultService _quizResultService;

        public QuizController(IQuizMasterApiService quizMasterApiService, IQuizTimerService timer, IQuizSessionService quizSessionService, IQuizResultService quizResultService)
        {
            _quizMasterApiService = quizMasterApiService;
            _quizTimer = timer;
            _quizSessionService = quizSessionService;
            _quizResultService = quizResultService;
        }

        public IActionResult Start(int amount, int? category, string? difficulty, string? type, string name)
        {
            _quizSessionService.ClearSession();
            _quizTimer.ResetTimer();
            _quizTimer.StartTimer();
            var questions = _quizMasterApiService.GetQuizQuestions(amount, category, difficulty, type).Result;

            _quizSessionService.SaveQuestionsToSession(questions);
            _quizSessionService.SaveNameToSession(name);

            return RedirectToAction("Question", new { index = 0 });

        }

        public IActionResult Question(int index)
        {
            var questionIds = _quizSessionService.GetQuestionIdsFromSession();

            if (index >= _quizSessionService.CountQuestionsInSession())
            {
                return RedirectToAction("Result");
            }

            var questionId = questionIds[index];
            var question = _quizSessionService.GetQuestionFromSessionById(questionId);

            var model = new QuestionViewModel
            {
                Id = question.Id,
                Question = question.QuestionText,
                Category = question.Category,
                Difficulty = question.Difficulty.Substring(0, 1).ToUpper() + question.Difficulty.Substring(1),
                Options = question.AnswerOptions.Select(o => o.Text).ToList(),
                CurrentIndex = index + 1,
                TotalQuestions = _quizSessionService.CountQuestionsInSession(),
                RemainingSeconds = _quizTimer.GetRemainingSeconds()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Question(QuestionViewModel model)
        {
            _quizSessionService.SaveAnswerToSession(model.Id, model.SelectedAnswer);

            if (model.CurrentIndex >= _quizSessionService.CountQuestionsInSession())
            {
                return RedirectToAction("Result");
            }

            return RedirectToAction("Question", new { index = model.CurrentIndex });
        }

        public IActionResult Result()
        {
            var totalSeconds = _quizTimer.GetTotalSeconds();
            _quizTimer.ResetTimer();

            var questions = _quizSessionService.GetQuestionsFromSession();

            var results = _quizResultService.EvaluateAnswers(questions);
            int score = results.Where(r => r.IsCorrect == true).Count();
            var percentage = _quizResultService.CalculatePercentage(score, results.Count);
            double points = _quizResultService.CalculateTotalPoints(results, totalSeconds, (int)_quizTimer.Duration.TotalSeconds);

            var name = _quizSessionService.GetNameFromSession();
            ViewBag.Qualification = _quizResultService.CheckQualification(points);

            _quizResultService.SaveScoreToJsonFile(name, score, points, results.Count, totalSeconds);
            string comment = _quizResultService.GenerateResultsComment(percentage);

            var model = new QuizResultViewModel
            {
                Name = name,
                Results = results,
                Score = score,
                Points = points,
                Percentage = percentage,
                Comment = comment,
                TotalQuestions = results.Count,
                TotalSeconds = totalSeconds
            };

            return View(model);
        }
    }
}
