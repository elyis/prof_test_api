using prof_tester_api.src.Domain.Entities.Response;

namespace prof_tester_api.src.Domain.Models
{
    public class OrganizationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public List<UserModel> Staff { get; set; } = new();
        public List<LecternModel> Lecterns { get; set; } = new();

        public OrganizationBody ToOrganizationBody()
        {
            return new OrganizationBody
            {
                Id = Id,
                Name = Name
            };
        }
    }
}