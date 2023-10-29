using Microsoft.EntityFrameworkCore;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.IRepository;
using prof_tester_api.src.Domain.Models;
using prof_tester_api.src.Infrastructure.Data;

namespace prof_tester_api.src.Infrastructure.Repository
{
    public class TestRepository : ITestRepository
    {
        private readonly AppDbContext _context;

        public TestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TestModel?> AddAsync(CreateTestBody body, DepartmentModel department)
        {
            var test = await _context.Tests
                .FirstOrDefaultAsync(e => e.DepartmentId == department.Id && e.Name == body.Name);

            if (test != null)
                return null;

            var newTest = new TestModel
            {
                Department = department,
                Name = body.Name,
                Questions = body.Questions.Select(e => new QuestionModel
                {
                    Text = e.Name,
                    Answers = e.Answers
                    .Select(answer => new AnswerModel
                    {
                        Text = answer,
                    }
                    ).ToList(),
                    RightAnswer = e.Answers[e.RightAnswerIndex]
                }
                ).ToList()
            };

            var result = await _context.Tests.AddAsync(newTest);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }

        public async Task<IEnumerable<TestModel>> GetAllAsync(Guid departmentId)
            => await _context.Tests
                .Include(e => e.Questions)
                .ThenInclude(e => e.Answers)
                .Where(e => e.DepartmentId == departmentId)
                .ToListAsync();

        public async Task<TestModel?> GetAsync(Guid testId)
            => await _context.Tests
                .Include(e => e.Questions)
                .ThenInclude(e => e.Answers)
                .FirstOrDefaultAsync(e => e.Id == testId);

        public async Task<TestResultModel?> CreateTestResult(CreateTestResultBody body, UserModel user)
        {
            var test = await GetAsync(body.TestId);
            if (test == null)
                return null;

            var newTestResult = new TestResultModel
            {
                RightCountAnswers = body.RightCountAnswer,
                Test = test,
                User = user
            };

            var result = await _context.TestResults.AddAsync(newTestResult);
            await _context.SaveChangesAsync();
            return result?.Entity;
        }


    }
}