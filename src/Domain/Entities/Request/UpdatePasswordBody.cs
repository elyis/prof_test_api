namespace prof_tester_api.src.Domain.Entities.Request
{
    public class UpdatePasswordBody
    {
        public string OldPassword { get; set; }
        public string Password { get; set; }
    }
}