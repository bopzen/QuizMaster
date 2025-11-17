using Microsoft.AspNetCore.Mvc;
using QuizMaster.Contracts;
using QuizMaster.ViewModels;

namespace QuizMaster.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IQuizMasterApiService _quizMasterApiService;
        private const int DefaultNumberOfQuestions = 10;

        public HomeController(ILogger<HomeController> logger, IQuizMasterApiService quizMasterApiService)
        {
            _logger = logger;
            _quizMasterApiService = quizMasterApiService;
        }

        public IActionResult Index()
        {
            var model = new QuizSettingsViewModel
            {
                NumberOfQuestions = DefaultNumberOfQuestions,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(QuizSettingsViewModel settings)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", settings);
            }

            settings.NumberOfQuestions = settings.NumberOfQuestions <= 0 ? DefaultNumberOfQuestions : settings.NumberOfQuestions;
            settings.Category = settings.Category.HasValue ? settings.Category : null;
            settings.Difficulty = string.IsNullOrWhiteSpace(settings.Difficulty) ? null : settings.Difficulty.ToLower();
            settings.Type = string.IsNullOrWhiteSpace(settings.Type) ? null : settings.Type.ToLower();

            return RedirectToAction("Start", "Quiz", new
            {
                amount = settings.NumberOfQuestions,
                category = (int?)settings.Category,
                difficulty = settings.Difficulty,
                type = settings.Type,
                name = settings.Name
            });
        }
    }
}
