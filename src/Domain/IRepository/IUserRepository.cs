using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Models;

namespace prof_tester_api.src.Domain.IRepository
{
    public interface IUserRepository
    {
        Task<UserModel?> AddAsync(SignUpBody body, string role, OrganizationModel organization, DepartmentModel? department);
        Task<UserModel?> GetAsync(Guid id);
        Task<UserModel?> UpdatePassword(Guid id, string password);
        Task<UserModel?> GetWithOrganizationAsync(Guid id);
        Task<UserModel?> GetAsync(string phone);
        Task<UserModel?> UpdateProfileAsync(Guid userId, UpdateProfileBody body);
        Task<bool> UpdateTokenAsync(string newRefreshToken, Guid id);
        Task<UserModel?> GetByTokenAsync(string refreshToken);
        Task<UserModel?> UpdateProfileIconAsync(Guid userId, string filename);
        Task<UserModel?> GetAsyncWithDepartment(Guid id);
        Task<UserModel?> GetAsyncWithTestResults(Guid id);
        Task<IEnumerable<UserModel>> GetAllByRoleWithTestResults(string rolename, Guid organizationId);
        Task<IEnumerable<UserModel>> GetAllWithTestResults(Guid organizationId, Guid departmentId);
    }
}