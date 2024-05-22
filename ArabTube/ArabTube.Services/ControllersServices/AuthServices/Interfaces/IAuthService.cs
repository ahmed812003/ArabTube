using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.GenericModels;

namespace ArabTube.Services.ControllersServices.AuthServices.Interfaces
{
    public interface IAuthService
    {
        Task<ProcessResult> RegisterAsync(RegisterDto model);

        Task<ProcessResult> EmailConfirmationAsync(string userId , string validCode , string userCode);

        Task<ProcessResult> ForgetPasswordAsync(string email);

        Task<ProcessResult> ResetPasswordAsync(string userId, string validCode, string userCode, string newPassword);

        Task<AuthResult> GetTokenAsync(LoginDto model);

        Task<ProcessResult> AddRoleAsync(RoleDto model);
        string GenerateOTP();

    }
}
