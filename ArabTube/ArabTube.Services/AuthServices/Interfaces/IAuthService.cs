using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.AuthServices.Interfaces
{
    public interface IAuthService
    {
        Task<RegisterResult> RegisterAsync(RegisterModel model);

        Task<AuthResult> GetTokenAsync(LoginModel model);

        Task<string> AddRoleAsync(RoleModel model);
    }
}
