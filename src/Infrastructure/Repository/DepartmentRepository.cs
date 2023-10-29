using Microsoft.EntityFrameworkCore;
using prof_tester_api.src.Domain.IRepository;
using prof_tester_api.src.Domain.Models;
using prof_tester_api.src.Infrastructure.Data;

namespace prof_tester_api.src.Infrastructure.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AppDbContext _context;

        public DepartmentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DepartmentModel?> AddAsync(string name, OrganizationModel organization)
        {
            var department = await GetAsync(name, organization.Id);
            if (department != null)
                return null;

            var newDepartment = new DepartmentModel
            {
                Name = name,
                Organization = organization,
            };

            var result = await _context.Departments.AddAsync(newDepartment);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<DepartmentModel>> GetAllAsync(Guid organizationId)
            => await _context.Departments
                .Where(e => e.OrganizationId == organizationId)
                .ToListAsync();

        public async Task<DepartmentModel?> GetAsync(Guid id)
            => await _context.Departments
                .Include(e => e.Organization)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<DepartmentModel?> GetAsync(string name, Guid organizationId)
            => await _context.Departments
                .FirstOrDefaultAsync(e => e.Name == name && e.OrganizationId == organizationId);

        public async Task<DepartmentModel?> GetWithTestResultsAsync(Guid id)
            => await _context.Departments
                .Include(e => e.Tests)
                    .ThenInclude(e => e.TestResults)
                        .ThenInclude(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<DepartmentModel?> GetWithUsersAsync(Guid id)
            => await _context.Departments
                .Include(e => e.Organization)
                .Include(e => e.Employees)
                .FirstOrDefaultAsync(e => e.Id == id);
    }
}