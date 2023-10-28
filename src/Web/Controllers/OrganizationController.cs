using Microsoft.AspNetCore.Mvc;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Entities.Response;
using prof_tester_api.src.Domain.IRepository;
using Swashbuckle.AspNetCore.Annotations;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationRepository _organizationRepository;

        public OrganizationController(IOrganizationRepository organozationRepository)
        {
            _organizationRepository = organozationRepository;
        }

        [HttpGet("organizations")]
        [SwaggerOperation("Получить список организаций")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<OrganizationBody>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _organizationRepository.GetAllAsync();
            return Ok(result.Select(e => e.ToOrganizationBody()));
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
        [SwaggerResponse(200, Type = typeof(OrganizationBody))]
        [SwaggerResponse(400, Description = "Данный номер привязан к другой учетке")]
        [SwaggerResponse(409)]

        public async Task<IActionResult> CreateOrganization(CreateOrganizationBody body)
        {
            var oldOrganization = await _organizationRepository.GetAsync(body.Name);
            if (oldOrganization != null)
                return Conflict();

            var result = await _organizationRepository.AddAsync(body);
            return result == null ? BadRequest() : Ok(result.ToOrganizationBody());
        }
    }
}