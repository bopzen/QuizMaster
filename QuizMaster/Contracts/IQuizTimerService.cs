namespace QuizMaster.Contracts
{
    public interface IQuizTimerService
    {
        TimeSpan Duration { get; }
        void StartTimer();
        int GetRemainingSeconds();
        int GetTotalSeconds();
        void ResetTimer();
    }
}
