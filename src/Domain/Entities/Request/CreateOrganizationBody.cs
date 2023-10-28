using System.ComponentModel.DataAnnotations;

namespace prof_tester_api.src.Domain.Entities.Request
{
    public class CreateOrganizationBody
    {
        public string Name { get; set; }
        [Phone]
        public string PhoneAdmin { get; set; }
        public string PasswordAdmin { get; set; }
    }
}