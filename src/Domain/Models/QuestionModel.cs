namespace prof_tester_api.src.Domain.Models
{
    public class QuestionModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string RightAnswer { get; set; }
        public List<AnswerModel> Answers { get; set; }
    }
}