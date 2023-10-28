using System.ComponentModel.DataAnnotations;

namespace prof_tester_api.src.Domain.Entities.Request
{
    public class UpdateProfileBody
    {
        [EmailAddress]
        public string? Email { get; set; }
        public string? Fullname { get; set; }
    }
}