using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.AuthServices.Interfaces
{
    public interface IAuthService
    {
        Task<ProcessResult> RegisterAsync(RegisterModel model);

        Task<ProcessResult> EmailConfirmationAsync(string userId, string code);

        Task<ProcessResult> ForgetPassword(string email);

        Task<ProcessResult> ResetPasswordAsync(string userId, string code, string newPassword);

        Task<AuthResult> GetTokenAsync(LoginModel model);

        Task<ProcessResult> AddRoleAsync(RoleModel model);
    }
}
