namespace prof_tester_api.src.Domain.Entities.Response
{
    public class TestBody
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CreateQuestion> Questions { get; set; }
    }

    public class CreateQuestion
    {
        public string Name { get; set; }
        public List<string> Answers { get; set; }
        public int RightAnswerIndex { get; set; }
    }
}