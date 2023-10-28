using prof_tester_api.src.Domain.Entities.Request;
using Microsoft.AspNetCore.Mvc;

namespace prof_tester_api.src.App.IService
{
    public interface IAuthService
    {
        Task<IActionResult> SignUp(SignUpBody body, string rolename, Guid adminId);
        Task<IActionResult> SignIn(SignInBody body);
        Task<IActionResult> RestoreToken(string refreshToken);
    }
}