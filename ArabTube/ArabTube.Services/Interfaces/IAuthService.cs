using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;

namespace ArabTube.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ProcessResult> RegisterAsync(RegisterDto model);

        Task<ProcessResult> EmailConfirmationAsync(string userId);

        Task<ProcessResult> ForgetPasswordAsync(string email);

        Task<ProcessResult> ResetPasswordAsync(string userId, string newPassword);

        Task<AuthResult> GetTokenAsync(LoginDto model);

        Task<ProcessResult> AddRoleAsync(RoleDto model);
        string GenerateOTP();

    }
}
