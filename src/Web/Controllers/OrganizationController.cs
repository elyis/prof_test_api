using Microsoft.AspNetCore.Mvc;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Entities.Response;
using prof_tester_api.src.Domain.Entities.Shared;
using prof_tester_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IJwtService _jwtService;

        public OrganizationController(
            IOrganizationRepository organozationRepository,
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            IJwtService jwtService
        )
        {
            _organizationRepository = organozationRepository;
            _departmentRepository = departmentRepository;
            _userRepository = userRepository;
            _jwtService = jwtService;
        }


        [HttpGet("organizations")]
        [SwaggerOperation("Получить список организаций")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<OrganizationBody>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _organizationRepository.GetAllAsync();
            return Ok(result.Select(e => e.ToOrganizationBody()) ?? new List<OrganizationBody>());
        }


        [HttpGet("organization/{orgId}")]
        [SwaggerOperation("Получить организацию по id")]
        [SwaggerResponse(200, Type = typeof(OrganizationBody))]
        [SwaggerResponse(404)]

        public async Task<IActionResult> GetAsync(Guid orgId)
        {
            var organization = await _organizationRepository.GetAsync(orgId);
            return organization == null ? NotFound() : Ok(organization.ToOrganizationBody());
        }

        [HttpPost("organization")]
        [SwaggerOperation("Создать организацию")]
        [SwaggerResponse(200, Type = typeof(TokenPair))]
        [SwaggerResponse(400, Description = "Данный номер привязан к другой учетке")]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateOrganization(CreateOrganizationBody body)
        {
            var oldOrganization = await _organizationRepository.GetAsync(body.Name);
            if (oldOrganization != null)
                return Conflict();

            var result = await _organizationRepository.AddAsync(body);

            var admin = result.Staff.First();
            var tokenPair = _jwtService.GenerateDefaultTokenPair(admin.Id, admin.OrganizationId, admin.RoleName);
            await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, admin.Id);
            return result == null ? BadRequest() : Ok(tokenPair);
        }

        [HttpGet("employers/{departmentId}")]
        [SwaggerOperation("Получить сотрудников")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<ProfileBody>))]
        public async Task<IActionResult> GetEmployers(Guid departmentId)
        {
            var response = await _departmentRepository.GetWithUsersAsync(departmentId);
            return response == null ? NotFound() : Ok(response.Employees.Select(e => e.ToProfileBody()).ToList() ?? new List<ProfileBody>());
        }
    }
}