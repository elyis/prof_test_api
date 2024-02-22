using prof_tester_api.src.App.IService;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Entities.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using webApiTemplate.src.App.IService;
using prof_tester_api.src.Domain.IRepository;
using System.ComponentModel.DataAnnotations;
using prof_tester_api.src.Domain.Enums;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;
        private readonly IUserRepository _userRepository;


        public AuthController(
            IAuthService authService,
            IJwtService jwtService,
            IUserRepository userRepository
        )
        {
            _authService = authService;
            _jwtService = jwtService;
            _userRepository = userRepository;
        }


        [SwaggerOperation("Регистрация сотрудника")]
        [SwaggerResponse(200, "Успешно создан")]
        [SwaggerResponse(400)]
        [SwaggerResponse(409, "Номер уже привязан")]

        [HttpPost("signup/employe"), Authorize(Roles = "Admin")]
        public async Task<IActionResult> SignUpEmployeAsync(
            [FromHeader(Name = "Authorization")] string token,
            SignUpBody signUpBody)
        {

            var tokenPayload = _jwtService.GetTokenPayload(token);
            string role = Enum.GetName(signUpBody.Role)!;
            var result = await _authService.SignUp(signUpBody, role, tokenPayload.UserId);
            return result;
        }


        [SwaggerOperation("Авторизация")]
        [SwaggerResponse(200, "Успешно", Type = typeof(TokenPairInfo))]
        [SwaggerResponse(400, "Пароли не совпадают")]
        [SwaggerResponse(404, "Email не зарегистрирован")]

        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync(SignInBody signInBody)
        {
            var result = await _authService.SignIn(signInBody);
            return result;
        }

        [SwaggerOperation("Восстановление токена")]
        [SwaggerResponse(200, "Успешно создан", Type = typeof(TokenPairInfo))]
        [SwaggerResponse(404, "Токен не используется")]

        [HttpPost("token")]
        public async Task<IActionResult> RestoreTokenAsync(TokenBody body)
        {
            var result = await _authService.RestoreToken(body.Value);
            return result;
        }

        [SwaggerOperation("Сбросить пароль")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [HttpPost("update-password"), Authorize]

        public async Task<IActionResult> UpdatePassword(
            UpdatePasswordBody body,
            [FromHeader(Name = "Authorization")] string token)
        {
            var tokenInfo = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.GetAsync(tokenInfo.UserId);
            if (user.Password != body.OldPassword)
                return BadRequest("oldPassword is not correct");

            var result = await _userRepository.UpdatePassword(tokenInfo.UserId, body.Password);
            return result == null ? BadRequest() : Ok();
        }

        [SwaggerOperation("Сбросить пароль")]
        [SwaggerResponse(200)]
        [SwaggerResponse(400)]
        [HttpPost("update-password/{userId}"), Authorize(Roles = "Admin")]

        public async Task<IActionResult> UpdatePasswordEmployer(
            UpdatePasswordBody body,
            [FromHeader(Name = "Authorization")] string token,
            [Required] Guid userId
        )
        {
            var user = await _userRepository.GetAsync(userId);
            if (user.RoleName == UserRole.Admin.ToString())
                return BadRequest("other admin account");

            if (user.Password != body.OldPassword)
                return BadRequest("oldPassword is not correct");

            var result = await _userRepository.UpdatePassword(userId, body.Password);
            return result == null ? BadRequest() : Ok();
        }

        [SwaggerOperation("Удалить сотрудника из организации"), Authorize(Roles = "Admin")]
        [SwaggerResponse(204)]
        [HttpDelete("remove-employer/{userId}")]

        public async Task<IActionResult> RemoveEmployer(
            [FromHeader(Name = "Authorization")] string token,
            [Required] Guid userId
        )
        {
            var tokenInfo = _jwtService.GetTokenPayload(token);
            var user = await _userRepository.GetAsync(userId);
            if (user == null)
                return NoContent();

            var result = await _userRepository.Remove(userId);
            return NoContent();
        }
    }
}