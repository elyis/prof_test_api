using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Models;

namespace prof_tester_api.src.Domain.IRepository
{
    public interface ILecternRepository
    {
        Task<LecternModel?> GetAsync(Guid id);
        Task<IEnumerable<LecternModel>> GetAllAsync(Guid organizationId);
        Task<LecternModel?> AddAsync(CreateLecternBody lecternBody, OrganizationModel organization);
        Task<bool> RemoveAsync(Guid id);
        Task<LecternModel?> UpdateFilenameAsync(Guid lecternId, string filename);
    }
}