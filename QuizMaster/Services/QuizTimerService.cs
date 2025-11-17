using QuizMaster.Contracts;

namespace QuizMaster.Services
{
    public class QuizTimerService : IQuizTimerService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeSpan _duration;

        public QuizTimerService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _duration = TimeSpan.FromMinutes(configuration.GetValue<int>("QuizDurationMinutes"));
        }

        public TimeSpan Duration { get { return _duration; } }

        private ISession Session => _httpContextAccessor.HttpContext!.Session;

        public void StartTimer()
        {
            Session.SetString("QuizStartTime", DateTime.UtcNow.ToString("O"));
        }

        public int GetRemainingSeconds()
        {
            var startTimeStr = Session.GetString("QuizStartTime");
            if (string.IsNullOrEmpty(startTimeStr))
                return (int)_duration.TotalSeconds;

            var startTime = DateTime.Parse(startTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var elapsed = DateTime.UtcNow - startTime;
            var remaining = _duration - elapsed;
            return remaining.TotalSeconds > 0 ? (int)remaining.TotalSeconds : 0;
        }

        public int GetTotalSeconds()
        {
            var startTimeStr = Session.GetString("QuizStartTime");
            if (string.IsNullOrEmpty(startTimeStr))
                return 0;

            var startTime = DateTime.Parse(startTimeStr, null, System.Globalization.DateTimeStyles.RoundtripKind);
            var elapsed = DateTime.UtcNow - startTime;
            return (int)elapsed.TotalSeconds;
        }

        public void ResetTimer()
        {
            Session.Remove("QuizStartTime");
        }
    }
}
