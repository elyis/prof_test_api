using prof_tester_api.src.Domain.Models;

namespace prof_tester_api.src.Domain.IRepository
{
    public interface IDepartmentRepository
    {
        Task<DepartmentModel?> GetAsync(Guid id);
        Task<DepartmentModel?> GetAsync(string name, Guid organizationId);
        Task<IEnumerable<DepartmentModel>> GetAllAsync(Guid organizationId);
        Task<DepartmentModel?> AddAsync(string name, OrganizationModel organization);
        Task<DepartmentModel?> GetWithUsersAsync(Guid id);
        Task<DepartmentModel?> GetWithTestResultsAsync(Guid id);
    }
}