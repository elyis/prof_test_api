namespace prof_tester_api.src.Domain.Entities.Response
{
    public class AnalyticsBody
    {
        public string Fullname { get; set; }
        public Guid UserId { get; set; }
        public int CountPoints { get; set; }
    }
}