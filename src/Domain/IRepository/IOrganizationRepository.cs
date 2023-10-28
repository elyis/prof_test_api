using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Models;

namespace prof_tester_api.src.Domain.IRepository
{
    public interface IOrganizationRepository
    {
        Task<OrganizationModel?> AddAsync(CreateOrganizationBody body);
        Task<OrganizationModel?> GetAsync(Guid id);
        Task<OrganizationModel?> GetAsync(string name);
        Task<IEnumerable<OrganizationModel>> GetAllAsync();
    }
}