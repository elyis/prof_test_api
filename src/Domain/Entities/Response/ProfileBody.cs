using prof_tester_api.src.Domain.Enums;

namespace prof_tester_api.src.Domain.Entities.Response
{
    public class ProfileBody
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public UserRole Role { get; set; }
        public string? Email { get; set; }
        public string? Fullname { get; set; }
        public string? UrlIcon { get; set; }

        public DepartmentBody? Department { get; set; }
    }
}