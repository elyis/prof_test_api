using Microsoft.EntityFrameworkCore;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Enums;
using prof_tester_api.src.Domain.IRepository;
using prof_tester_api.src.Domain.Models;
using prof_tester_api.src.Infrastructure.Data;

namespace prof_tester_api.src.Infrastructure.Repository
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext _context;

        public OrganizationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OrganizationModel?> AddAsync(CreateOrganizationBody body)
        {
            var organization = await GetAsync(body.Name);
            if (organization != null)
                return null;

            var newOrganization = new OrganizationModel
            {
                Name = body.Name,
            };

            var organizationAdmin = new UserModel
            {
                Phone = body.PhoneAdmin,
                Password = body.PasswordAdmin,
                RoleName = Enum.GetName(UserRole.Admin)!,
                Organization = newOrganization
            };

            var newOrganizationResult = await _context.Organizations.AddAsync(newOrganization);
            var userWithPhone = await _context.Users.FirstOrDefaultAsync(e => e.Phone == body.PhoneAdmin);
            if (userWithPhone != null)
                return null;

            await _context.Users.AddAsync(organizationAdmin);
            await _context.SaveChangesAsync();
            return newOrganizationResult?.Entity;
        }

        public async Task<IEnumerable<OrganizationModel>> GetAllAsync()
            => await _context.Organizations
                .ToListAsync();

        public async Task<OrganizationModel?> GetAsync(Guid id)
            => await _context.Organizations
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<OrganizationModel?> GetAsync(string name)
            => await _context.Organizations
                .FirstOrDefaultAsync(e => e.Name == name);
    }
}