namespace prof_tester_api.src.Domain.Entities.Response
{
    public class TestAnalyticBody
    {
        public Guid Id { get; set; }
        public string TestName { get; set; }
        public int AverageCountPoints { get; set; }
        public int MaxCountPointsByTest { get; set; }
        public bool IsCompleted { get; set; }
    }
}