using System.ComponentModel.DataAnnotations;

namespace prof_tester_api.src.Domain.Entities.Request
{
    public class UpdateEmployerProfileBody
    {
        [Required]
        public Guid EmployerId { get; set; }
        [EmailAddress]
        public string? Email { get; set; }
        [Required, Phone]
        public string Phone { get; set; }
        [Required]
        public Guid DepartmentId { get; set; }
        public string? Fullname { get; set; }
    }
}