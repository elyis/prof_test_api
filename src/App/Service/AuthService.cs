using prof_tester_api.src.App.IService;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using webApiTemplate.src.App.IService;

namespace prof_tester_api.src.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> RestoreToken(string refreshToken)
        {
            var oldUser = await _userRepository.GetByTokenAsync(refreshToken);
            if (oldUser == null)
                return new NotFoundResult();

            var tokenPair = _jwtService.GenerateDefaultTokenPair(oldUser.Id, oldUser.OrganizationId, oldUser.RoleName);
            await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);

            return new OkObjectResult(tokenPair);
        }

        public async Task<IActionResult> SignIn(SignInBody body)
        {
            var oldUser = await _userRepository.GetAsync(body.Phone);
            if (oldUser == null)
                return new NotFoundResult();

            if (oldUser.Password != body.Password)
                return new BadRequestResult();

            var tokenPair = _jwtService.GenerateDefaultTokenPair(oldUser.Id, oldUser.OrganizationId, oldUser.RoleName);
            await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);
            return new OkObjectResult(tokenPair);
        }

        public async Task<IActionResult> SignUp(SignUpBody body, string rolename, Guid adminId)
        {
            var oldUser = await _userRepository.AddAsync(body, rolename, adminId);
            if (oldUser == null)
                return new ConflictResult();


            var tokenPair = _jwtService.GenerateDefaultTokenPair(oldUser.Id, oldUser.OrganizationId, rolename);
            await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);
            return new OkObjectResult(tokenPair);
        }
    }
}