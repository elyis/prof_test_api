using prof_tester_api.src.Domain.Entities.Response;

namespace prof_tester_api.src.Domain.Models
{
    public class TestModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public DepartmentModel Department { get; set; }
        public Guid DepartmentId { get; set; }
        public List<QuestionModel> Questions { get; set; } = new();
        public List<TestResultModel> TestResults { get; set; } = new();

        public TestBody ToTestBody()
        {
            return new TestBody
            {
                Id = Id,
                Name = Name,
                Questions = Questions.Select(e => new CreateQuestion
                {
                    Name = e.Text,
                    Answers = e.Answers.Select(e => e.Text).ToList(),
                    RightAnswerIndex = e.Answers.FindIndex(s => s.Text == e.RightAnswer)
                })
                .ToList()
            };
        }
    }
}