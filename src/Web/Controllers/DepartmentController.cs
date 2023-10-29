using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Entities.Response;
using prof_tester_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IJwtService _jwtService;

        public DepartmentController(
            IDepartmentRepository departmentRepository,
            IOrganizationRepository organizationRepository,
            IJwtService jwtService)
        {
            _departmentRepository = departmentRepository;
            _organizationRepository = organizationRepository;
            _jwtService = jwtService;
        }

        [HttpGet("departments"), Authorize]
        [SwaggerOperation("Получить список отделов")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<DepartmentBody>))]
        public async Task<IActionResult> GetOrganization([FromHeader(Name = "Authorization")] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var result = await _departmentRepository.GetAllAsync(tokenPayload.OrganizationId);
            return Ok(result.Select(e => e.ToDepartmentBody()) ?? new List<DepartmentBody>());
        }

        [HttpGet("department/{id}"), Authorize]
        [SwaggerOperation("Получить отдел по id")]
        [SwaggerResponse(200, Type = typeof(DepartmentEmployeesBody))]
        public async Task<IActionResult> GetOrganization(
            [FromHeader(Name = "Authorization")] string token,
            Guid id
        )
        {
            var result = await _departmentRepository.GetWithUsersAsync(id);
            return result == null ? NotFound() : Ok(result.ToDepartmentEmployeesBody());
        }

        [HttpPost("department")]
        [SwaggerOperation("Создать отдел")]
        [SwaggerResponse(200, Type = typeof(DepartmentBody))]
        [SwaggerResponse(409, Description = "В данной организации уже существует такой отдел")]

        public async Task<IActionResult> CreateDepartment(
            [FromHeader(Name = "Authorization")] string token,
            CreateDepartmentBody body)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var organization = await _organizationRepository.GetAsync(tokenPayload.OrganizationId);
            var department = await _departmentRepository.AddAsync(body.Name, organization);
            return department == null ? Conflict() : Ok(department.ToDepartmentBody());
        }
    }
}