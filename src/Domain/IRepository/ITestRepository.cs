using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Models;

namespace prof_tester_api.src.Domain.IRepository
{
    public interface ITestRepository
    {
        Task<TestModel?> AddAsync(CreateTestBody body, DepartmentModel department);
        Task<IEnumerable<TestModel>> GetAllAsync(Guid departmentId);
        Task<TestModel?> GetAsync(Guid testId);
        Task<TestResultModel?> CreateTestResult(CreateTestResultBody body, UserModel user);
    }
}