namespace QuizMaster.Models
{
    public class QuizMasterApiResponseModel
    {
        public int ResponseCode { get; set; }
        public List<QuestionModel> Results { get; set; } = new();
    }
}
