using prof_tester_api.src.App.IService;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.Entities.Shared;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;
using webApiTemplate.src.App.IService;

namespace prof_tester_api.src.Web.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtService _jwtService;


        public AuthController(
            IAuthService authService,
            IJwtService jwtService
        )
        {
            _authService = authService;
            _jwtService = jwtService;
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

    }
}