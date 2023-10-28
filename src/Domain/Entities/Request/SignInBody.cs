using System.ComponentModel.DataAnnotations;

namespace prof_tester_api.src.Domain.Entities.Request
{
    public class SignInBody
    {
        [Phone]
        [Required]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }
    }
}