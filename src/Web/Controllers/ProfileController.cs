using prof_tester_api.src.Domain.Entities.Response;
using prof_tester_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using webApiTemplate.src.App.IService;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IJwtService _jwtService;

        public ProfileController(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            IJwtService jwtService)
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _jwtService = jwtService;
        }


        [HttpGet("profile"), Authorize]
        [SwaggerOperation("Получить профиль")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        public async Task<IActionResult> GetProfileAsync([FromHeader(Name = "Authorization")] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.GetAsync(tokenPayload.UserId);
            return user == null ? NotFound() : Ok(user.ToProfileBody());
        }

        [HttpGet("users"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Получить всех сотрудников")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(IEnumerable<ProfileBody>))]
        public async Task<IActionResult> GetUsers([FromHeader(Name = "Authorization")] string token)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var users =
                (await _userRepository.GetAll(tokenPayload.OrganizationId))
                .Where(e => e.RoleName != UserRole.Admin.ToString());

            var result = users.Select(e => e.ToProfileBody());
            return Ok(result);
        }

        [HttpPut("profile/{userId}"), Authorize(Roles = "Admin")]
        [SwaggerOperation("Обновить профиль сотрудника")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        [SwaggerResponse(400)]
        [SwaggerResponse(404, Description = "Пользователь не найден")]
        public async Task<IActionResult> UpdateProfileAsync(
            UpdateEmployerProfileBody body,
            [Required] Guid userId)
        {
            var department = await _departmentRepository.GetAsync(body.DepartmentId);
            if (department == null)
                return BadRequest();

            var user = await _userRepository.UpdateProfileAsync(userId, body, department);
            return user == null ? NotFound() : Ok(user.ToProfileBody());
        }


        [HttpPut("profile"), Authorize]
        [SwaggerOperation("Обновить профиль")]
        [SwaggerResponse(200, Description = "Успешно", Type = typeof(ProfileBody))]
        [SwaggerResponse(404, Description = "Пользователь не найден")]
        public async Task<IActionResult> UpdateProfileAsync(
            [FromHeader(Name = "Authorization")] string token,
            UpdateProfileBody body)
        {
            var tokenPayload = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.UpdateProfileAsync(tokenPayload.UserId, body);
            return user == null ? NotFound() : Ok(user.ToProfileBody());
        }
    }
}