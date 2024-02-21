namespace prof_tester_api.src.Domain.Entities.Response
{
    public class TestAnalyticBody
    {
        public string TestName { get; set; }
        public int AverageCountPoints { get; set; }
        public int MaxCountPoints { get; set; }
        public bool IsCompleted { get; set; }
    }
}