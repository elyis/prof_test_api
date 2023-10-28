namespace prof_tester_api.src.Domain.Entities.Shared
{
    public class TokenPayload
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
    }
}