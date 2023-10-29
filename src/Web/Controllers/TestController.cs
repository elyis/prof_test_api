using Microsoft.AspNetCore.Mvc;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Entities.Response;
using prof_tester_api.src.Domain.Enums;
using prof_tester_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class TestController : ControllerBase
    {
        private readonly ITestRepository _testRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public TestController(
            ITestRepository testRepository,
            IDepartmentRepository departmentRepository,
            IUserRepository userRepository,
            IJwtService jwtService
        )
        {
            _departmentRepository = departmentRepository;
            _testRepository = testRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        [HttpPost("test")]
        [SwaggerOperation("Создать тест")]
        [SwaggerResponse(200, Type = typeof(TestBody))]
        [SwaggerResponse(400, Description = "Неверный идентификатор отдел")]
        [SwaggerResponse(409, Description = "Тест с данным именем существует для этого отдела")]
        public async Task<IActionResult> CreateTest(
            CreateTestBody body
        )
        {
            var department = await _departmentRepository.GetAsync(body.DepartmentId);
            if (department == null)
                return BadRequest();

            var test = await _testRepository.AddAsync(body, department);
            if (test == null)
                return Conflict();

            return Ok();
        }

        [HttpGet("tests/{departmentId}")]
        [SwaggerOperation("Получить список доступных тестов")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TestInfo>))]
        [SwaggerResponse(400, Description = "Не прикреплен к отделу")]
        public async Task<IActionResult> GetAll(
            [FromHeader(Name = "Authorization")] string token,
            Guid departmentId
        )
        {
            var result = await _testRepository.GetAllAsync(departmentId);
            return Ok(result.Select(e => new TestInfo
            {
                Id = e.Id,
                Name = e.Name
            }) ?? new List<TestInfo>());
        }


        [HttpGet("tests")]
        [SwaggerOperation("Получить список доступных тестов")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TestInfo>))]
        [SwaggerResponse(400, Description = "Не прикреплен к отделу")]
        public async Task<IActionResult> GetAll(
            [FromHeader(Name = "Authorization")] string token
        )
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.GetAsyncWithDepartment(tokenPayload.UserId);
            if (user.DepartmentId == null)
                return BadRequest();

            var result = await _testRepository.GetAllAsync((Guid)user.DepartmentId);
            return Ok(result.Select(e => new TestInfo
            {
                Id = e.Id,
                Name = e.Name
            }) ?? new List<TestInfo>());
        }

        [HttpGet("test/{testId}")]
        [SwaggerOperation("Получить тест по id")]
        [SwaggerResponse(200, Type = typeof(TestBody))]
        [SwaggerResponse(404)]
        public async Task<IActionResult> GetTestById(Guid testId)
        {
            var result = await _testRepository.GetAsync(testId);
            return result == null ? NotFound() : Ok(result.ToTestBody());
        }


        [HttpPost("test/result")]
        [SwaggerOperation("Создать запись результата прохождения теста")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400, Description = "Неверный идентификатор")]
        public async Task<IActionResult> CreateTestResultAsync(
            [FromHeader(Name = "Authorization")] string token,
            CreateTestResultBody body
        )
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.GetAsync(tokenPayload.UserId);
            var result = await _testRepository.CreateTestResult(body, user);
            return result == null ? BadRequest() : Ok();
        }

        [HttpGet("tests/analytic/{departmentId}")]
        [SwaggerOperation("Получить аналитику по отделу")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<AnalyticsBody>))]

        public async Task<IActionResult> GetAnalytics(
            [FromHeader(Name = "Authorization")] string token,
            Guid departmentId)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var users = await _userRepository.GetAllWithTestResults(tokenPayload.OrganizationId, departmentId);

            var response = users.Select(user => new AnalyticsBody
            {
                Fullname = user.Fullname,
                UserId = user.Id,
                CountPoints = (int)user.TestResults.GroupBy(e => e.TestId).Select(e => e.Average(e => e.RightCountAnswers)).Sum()
            })
            .OrderByDescending(e => e.CountPoints)
            .ToList();

            return Ok(response ?? new List<AnalyticsBody>());
        }

        [HttpGet("tests/analytics/")]
        [SwaggerOperation("Получить аналитику по тестам")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TestAnalyticBody>))]

        public async Task<IActionResult> GetAnalytics(
            [FromHeader(Name = "Authorization")] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.GetAsyncWithTestResults(tokenPayload.UserId);

            var response = user.TestResults.GroupBy(e => e.Test.Name).Select(test => new TestAnalyticBody
            {
                TestName = test.Key,
                CountPoints = (int)test.Average(t => t.RightCountAnswers),
                MaxCountPoints = test.First().Test.Questions.Count
            })
            .OrderByDescending(e => e.TestName)
            .ToList();

            return Ok(response ?? new List<TestAnalyticBody>());
        }

        [HttpGet("tests/analytics/{userId}")]
        [SwaggerOperation("Получить аналитику по тестам")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<TestAnalyticBody>))]

        public async Task<IActionResult> GetAnalyticsById(Guid userId)
        {
            var user = await _userRepository.GetAsyncWithTestResults(userId);
            if (user == null)
                return NotFound();

            var response = user.TestResults.GroupBy(e => e.Test.Name).Select(test => new TestAnalyticBody
            {
                TestName = test.Key,
                CountPoints = (int)test.Average(t => t.RightCountAnswers),
                MaxCountPoints = test.First().Test.Questions.Count
            });
            return Ok(response ?? new List<TestAnalyticBody>());
        }
    }
}