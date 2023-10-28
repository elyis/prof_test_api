using System.ComponentModel.DataAnnotations;
using prof_tester_api.src.Domain.Entities.Response;
using prof_tester_api.src.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace prof_tester_api.src.Domain.Models
{
    [Index(nameof(Phone), IsUnique = true)]
    [Index(nameof(Token))]
    public class UserModel
    {
        public Guid Id { get; set; }

        [StringLength(256, MinimumLength = 3)]
        public string Phone { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public string? RestoreCode { get; set; }
        public string? Fullname { get; set; }
        public string? Email { get; set; }
        public DateTime? RestoreCodeValidBefore { get; set; }
        public string? Token { get; set; }
        public string? FilenameIcon { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public OrganizationModel Organization { get; set; }
        public Guid OrganizationId { get; set; }

        public ProfileBody ToProfileBody()
        {
            return new ProfileBody
            {
                Phone = Phone,
                Role = Enum.Parse<UserRole>(RoleName),
                UrlIcon = string.IsNullOrEmpty(FilenameIcon) ? null : $"{Constants.webPathToProfileIcons}{FilenameIcon}",
                Email = Email,
                Fullname = Fullname
            };
        }
    }
}