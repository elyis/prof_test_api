namespace prof_tester_api.src.Domain.Models
{
    public class AnswerModel
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public QuestionModel Question { get; set; }
        public Guid QuestionId { get; set; }
    }
}