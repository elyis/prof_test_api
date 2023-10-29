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
    public class LecternController : ControllerBase
    {
        private readonly ILecternRepository _lecternRepository;
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IJwtService _jwtService;

        public LecternController(
            ILecternRepository lecternRepository,
            IOrganizationRepository organizationRepository,
            IJwtService jwtService
        )
        {
            _lecternRepository = lecternRepository;
            _organizationRepository = organizationRepository;
            _jwtService = jwtService;
        }

        [HttpGet("lecterns"), Authorize]
        [SwaggerOperation("Получить список лекций в организации")]
        [SwaggerResponse(200, Type = typeof(IEnumerable<LecternBody>))]

        public async Task<IActionResult> GetLecterns([FromHeader(Name = "Authorization")] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);

            var lecterns = await _lecternRepository.GetAllAsync(tokenPayload.OrganizationId);
            return Ok(lecterns.Select(e => e.ToLecternBody()) ?? new List<LecternBody>());
        }


        [HttpGet("lectern/{lecternId}"), Authorize]
        [SwaggerOperation("Получить лекцию по id")]
        [SwaggerResponse(200, Type = typeof(LecternBody))]
        [SwaggerResponse(404)]

        public async Task<IActionResult> GetByIdAsync(Guid lecternId)
        {
            var lectern = await _lecternRepository.GetAsync(lecternId);
            return lectern == null ? NotFound() : Ok(lectern.ToLecternBody());
        }

        [HttpPost("lectern")]
        [SwaggerOperation("Создать новую лекцию")]
        [SwaggerResponse(200, Type = typeof(LecternBody))]

        public async Task<IActionResult> CreateLecternAsync(
            [FromHeader(Name = "Authorization")] string token,
            CreateLecternBody lecternBody
            )
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var organization = await _organizationRepository.GetAsync(tokenPayload.OrganizationId);
            var newLectern = await _lecternRepository.AddAsync(lecternBody, organization!);
            return Ok(newLectern?.ToLecternBody());
        }

        [HttpDelete("lectern/{lecternId}"), Authorize]
        [SwaggerOperation("Удалить лекцию по id")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]

        public async Task<IActionResult> RemoveByIdAsync(Guid lecternId)
        {
            var lectern = await _lecternRepository.RemoveAsync(lecternId);
            return lectern ? NoContent() : BadRequest();
        }

    }
}