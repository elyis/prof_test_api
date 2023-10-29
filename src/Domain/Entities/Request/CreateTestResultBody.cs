namespace prof_tester_api.src.Domain.Entities.Request
{
    public class CreateTestResultBody
    {
        public Guid TestId { get; set; }
        public int RightCountAnswer { get; set; }
    }
}