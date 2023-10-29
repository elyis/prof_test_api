using System.ComponentModel.DataAnnotations;
using prof_tester_api.src.Domain.Enums;

namespace prof_tester_api.src.Domain.Entities.Request
{
    public class SignUpBody
    {
        [Phone]
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Fullname { get; set; }
        public Guid DepartmentId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }
    }
}