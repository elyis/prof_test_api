using prof_tester_api.src.Domain.Entities.Response;

namespace prof_tester_api.src.Domain.Models
{
    public class LecternModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Filename { get; set; }

        public OrganizationModel Organization { get; set; }
        public Guid OrganizationId { get; set; }

        public LecternBody ToLecternBody()
        {
            return new LecternBody
            {
                Id = Id,
                Name = Name,
                UrlFile = Filename == null ? null : $"{Constants.webPathToLecternFile}{Filename}",
            };
        }
    }
}