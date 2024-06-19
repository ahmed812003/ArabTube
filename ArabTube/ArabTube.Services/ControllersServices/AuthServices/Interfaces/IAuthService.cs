using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.UserModels;

namespace ArabTube.Services.ControllersServices.AuthServices.Interfaces
{
    public interface IAuthService
    {
        Task<SecurityResult> RegisterAsync(RegisterDto model);

        Task<ProcessResult> EmailConfirmationAsync(string userId , string validCode , string userCode);

        Task<SecurityResult> ForgetPasswordAsync(string email);

        Task<ProcessResult> ResetPasswordAsync(string userId, string validCode, string userCode, string newPassword);

        Task<AuthResult> GetTokenAsync(LoginDto model);

        Task<ProcessResult> AddRoleAsync(RoleDto model);

        Task<SecurityResult> ResendEmailConfirmationAsync(ResendEmailConfirmationDto model);
        string GenerateOTP();

    }
}
