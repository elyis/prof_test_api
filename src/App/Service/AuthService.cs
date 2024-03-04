using prof_tester_api.src.App.IService;
using prof_tester_api.src.Domain.Entities.Request;
using prof_tester_api.src.Domain.IRepository;
using Microsoft.AspNetCore.Mvc;
using webApiTemplate.src.App.IService;
using prof_tester_api.src.Domain.Entities.Shared;
using prof_tester_api.src.Domain.Enums;

namespace prof_tester_api.src.App.Service
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IJwtService _jwtService;

        public AuthService(
            IUserRepository userRepository,
            IDepartmentRepository departmentRepository,
            IJwtService jwtService
        )
        {
            _userRepository = userRepository;
            _departmentRepository = departmentRepository;
            _jwtService = jwtService;
        }

        public async Task<IActionResult> RestoreToken(string refreshToken)
        {
            var oldUser = await _userRepository.GetByTokenAsync(refreshToken);
            if (oldUser == null)
                return new NotFoundResult();

            var tokenPair = _jwtService.GenerateDefaultTokenPair(oldUser.Id, oldUser.OrganizationId, oldUser.RoleName);
            await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);

            var tokenPairInfo = new TokenPairInfo
            {
                TokenPair = tokenPair,
                Role = Enum.Parse<UserRole>(oldUser.RoleName),
            };

            return new OkObjectResult(tokenPairInfo);
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

            var tokenPairInfo = new TokenPairInfo
            {
                TokenPair = tokenPair,
                Role = Enum.Parse<UserRole>(oldUser.RoleName),
            };
            return new OkObjectResult(tokenPairInfo);
        }

        public async Task<IActionResult> SignUp(SignUpBody body, string rolename, Guid adminId)
        {
            var admin = await _userRepository.GetWithOrganizationAsync(adminId);

            var department = await _departmentRepository.GetAsync(body.DepartmentId) ?? await _departmentRepository.AddAsync("Управляющие", admin.Organization);
            var oldUser = await _userRepository.AddAsync(body, rolename, admin.Organization, department);
            if (oldUser == null)
                return new ConflictResult();


            var tokenPair = _jwtService.GenerateDefaultTokenPair(oldUser.Id, oldUser.OrganizationId, rolename);
            await _userRepository.UpdateTokenAsync(tokenPair.RefreshToken, oldUser.Id);
            return new OkResult();
        }
    }
}