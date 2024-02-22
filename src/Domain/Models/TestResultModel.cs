namespace prof_tester_api.src.Domain.Models
{
    public class TestResultModel
    {
        public Guid Id { get; set; }
        public int RightCountAnswers { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public UserModel User { get; set; }
        public Guid UserId { get; set; }
        public TestModel Test { get; set; }
        public Guid TestId { get; set; }
    }
}