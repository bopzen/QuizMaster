using QuizMaster.Contracts;
using QuizMaster.Models;
using QuizMaster.ViewModels;

namespace QuizMaster.Services
{
    public class QuizResultService : IQuizResultService
    {
        private readonly IQuizSessionService _quizSessionService;
        private readonly IConfiguration _configuration;
        private readonly string _scoreFilePath;
        private readonly int _scoreRecordsCount;

        private const double BaseScale = 1000.0;
        private const double MinLengthFactor = 0.3;
        private const double LengthPower = 2.0;
        private const int MaxQuestions = 30;
        private const double TimeBonusWeight = 0.3;
        private const double PerfectBonusPercent = 0.1;

        public QuizResultService(IQuizSessionService quizSessionService, IConfiguration configuration)
        {
            _quizSessionService = quizSessionService;
            _configuration = configuration;
            _scoreFilePath = _configuration.GetValue<string>("ScoreFilePath") ?? "scores.json";
            _scoreRecordsCount = _configuration.GetValue<int>("ScoreRecordsCount");
        }

        public string GenerateResultsComment(double percentage)
        {
            string comment;
            switch (percentage)
            {
                case < 50:
                    comment = "Don't worry, keep learning and you'll improve!";
                    break;
                case < 75:
                    comment = "Good effort! A little more practice and you'll get even better.";
                    break;
                case < 90:
                    comment = "Great job! You're very close to perfection.";
                    break;
                default:
                    comment = "Excellent work! You're a true genius!";
                    break;
            }
            return comment;
        }

        public List<QuizResultItem> EvaluateAnswers(List<QuestionModel> questions)
        {
            var results = new List<QuizResultItem>();

            foreach (var q in questions)
            {
                var userAnswer = _quizSessionService.GetAnswerFromSession(q.Id);

                var correctAnswer = q.CorrectAnswer;
                bool isAnswerCorrect = userAnswer == correctAnswer;

                results.Add(new QuizResultItem
                {
                    Question = q.QuestionText,
                    Difficulty = q.Difficulty,
                    UserAnswer = userAnswer,
                    CorrectAnswer = correctAnswer,
                    IsCorrect = isAnswerCorrect,
                });
            }
            return results;
        }

        public double CalculatePercentage(int score, int totalQuestions)
        {
            if (totalQuestions == 0)
            {
                return 0;
            }

            return (double)score / (double)totalQuestions * 100;
        }

        public double CalculateTotalPoints(List<QuizResultItem> results, int totalSeconds, int allowedSeconds)
        {
            if (results == null)
            {
                return 0;
            }
            if (results.Count == 0)
            {
                return 0;
            }

            double totalWeight = results.Sum(r => GetPointsByDifficulty(r.Difficulty));
            double correctWeight = results
                .Where(r => r.IsCorrect)
                .Sum(r => GetPointsByDifficulty(r.Difficulty));

            double weightedAccuracy = (totalWeight > 0) ? (correctWeight / totalWeight) : 0;

            int totalQuestions = results.Count;
            double lengthFactor = CalculateLengthFactor(totalQuestions);

            double timeUsedRatio = Math.Clamp((double)totalSeconds / allowedSeconds, 0, 1);
            double timeFactor = 1.0 + TimeBonusWeight * (1.0 - timeUsedRatio);

            double baseScore = BaseScale * weightedAccuracy * lengthFactor * timeFactor;

            double perfectBonus = weightedAccuracy == 1.0 ? BaseScale * PerfectBonusPercent : 0;

            return Math.Round(baseScore + perfectBonus, 2);
        }

        private double GetPointsByDifficulty(string difficulty)
        {
            return difficulty.ToLower() switch
            {
                "easy" => 1.0,
                "medium" => 2.0,
                "hard" => 3.0,
                _ => 0,
            };
        }

        private double CalculateLengthFactor(int totalQuestions)
        {
            totalQuestions = Math.Max(1, Math.Min(totalQuestions, MaxQuestions));

            double normalized = Math.Log(totalQuestions + 1) / Math.Log(MaxQuestions + 1);

            double scaled = Math.Pow(normalized, LengthPower);

            return MinLengthFactor + (1.0 - MinLengthFactor) * scaled;
        }

        public void SaveScoreToJsonFile(string name, int score, double points, int totalQuestions, int totalSeconds)
        {
            if (points <= 0)
            {
                return;
            }
            var recordsCount = GetRecordCountFromJsonFile();
            if (CheckScoreRecord(points) || recordsCount < _scoreRecordsCount)
            {
                DeleteMinScoreFromJsonFile();

                var quizScoreRecord = new QuizScoreRecord
                {
                    Name = name,
                    Score = score,
                    Points = points,
                    TotalQuestions = totalQuestions,
                    TotalSeconds = totalSeconds,
                    DateAchieved = DateTime.Now
                };
                var records = ReadAllScoresFromJsonFile();
                records.Add(quizScoreRecord);
                var json = System.Text.Json.JsonSerializer.Serialize(records, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_scoreFilePath, json);
            }
        }

        public string CheckQualification(double points)
        {
            var maxScore = GetMaxScoreFromJsonFile();
            var minScore = GetMinScoreFromJsonFile();

            if (points > maxScore)
            {
                return "NewHighScore";
            }
            else if (points > minScore)
            {
                return "Qualified";
            }
            return "NotQualified";
        }

        public bool CheckScoreRecord(double points)
        {
            return points > GetMinScoreFromJsonFile();
        }

        public List<QuizScoreRecord> GetLeaderBoardScores()
        {
            var records = ReadAllScoresFromJsonFile();
            return records
                .OrderByDescending(r => r.Points)
                .ThenBy(r => r.TotalSeconds)
                .Take(_scoreRecordsCount)
                .ToList();
        }

        private List<QuizScoreRecord> ReadAllScoresFromJsonFile()
        {
            if (!File.Exists(_scoreFilePath))
            {
                return new List<QuizScoreRecord>();
            }

            try
            {
                var json = File.ReadAllText(_scoreFilePath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    return new List<QuizScoreRecord>();
                }
                return System.Text.Json.JsonSerializer.Deserialize<List<QuizScoreRecord>>(json) ?? new List<QuizScoreRecord>();
            }
            catch
            {
                return new List<QuizScoreRecord>();
            }
        }

        private int GetRecordCountFromJsonFile()
        {
            var records = ReadAllScoresFromJsonFile();
            return records.Count;
        }

        private double GetMaxScoreFromJsonFile()
        {
            var records = ReadAllScoresFromJsonFile();
            return records.Count > 0 ? records.Max(r => r.Points) : 0;
        }

        private double GetMinScoreFromJsonFile()
        {
            var records = ReadAllScoresFromJsonFile();
            return records.Count > 0 ? records.Min(r => r.Points) : 0;
        }

        private void DeleteMinScoreFromJsonFile()
        {
            var records = ReadAllScoresFromJsonFile();
            if (records.Count >= _scoreRecordsCount)
            {
                var minRecord = records.MinBy(r => r.Points);
                if (minRecord != null)
                {
                    records.Remove(minRecord);
                    var json = System.Text.Json.JsonSerializer.Serialize(records, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(_scoreFilePath, json);
                }
            }
        }
    }
}
