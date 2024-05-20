using ArabTube.Entities.AuthModels;
using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.AuthServices.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArabTube.Services.AuthServices.ImplementationClasses
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;

        public AuthService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, 
                          IConfiguration configuration, IMapper mapper, IEmailSender emailSender)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _mapper = mapper;
            _emailSender = emailSender;
        }

        //done
        // Register new user without authentication
        public async Task<ProcessResult> RegisterAsync(RegisterDto model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) != null)
                return new ProcessResult { Message = "This Email Already Registered!" };

            if (await _userManager.FindByNameAsync(model.UserName) != null)
                return new ProcessResult { Message = "This Username Already Registered!" };

            if (!(char.ToLower(model.UserName[0]) >= 'a' && char.ToLower(model.UserName[0]) <= 'z'))
                return new ProcessResult { Message = "Invalid username!" };

            var user = _mapper.Map<AppUser>(model);

            var Result = await _userManager.CreateAsync(user, model.Password);
            if (!Result.Succeeded)
            {
                string Error = string.Empty;
                foreach (var error in Result.Errors)
                    Error += $"{error.Description} , ";
                return new ProcessResult { Message = Error };
            }

            await _userManager.AddToRoleAsync(user, "User");

            return new ProcessResult
            {
                Message = user.Id,
                IsSuccesed = true
            };
        }

        // done 
        // login user and get Token
        public async Task<AuthResult> GetTokenAsync(LoginDto model)
        {
            var authModel = new AuthResult();

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.message = "Email or Password is incorrect!";
                return authModel;
            }

            if (!user.EmailConfirmed)
            {
                authModel.message = "Email needed To be Comfirmed";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = model.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            return authModel;
        }

        // done
        public async Task<ProcessResult> EmailConfirmationAsync(string userId)
        {
            if (userId == null)
            {
                return new ProcessResult { Message = "Invalid Email" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ProcessResult { Message = $"Unable to load user with ID '{userId}'." };
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var result = await _userManager.ConfirmEmailAsync(user, code);

            var processResult = new ProcessResult();
            processResult.Message = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            processResult.IsSuccesed = result.Succeeded;

            return processResult;
        }

        //done
        public async Task<ProcessResult> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return new ProcessResult { Message = "Email Is Not Found" };

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return new ProcessResult { Message = "No User With This Email" };

            if (!user.EmailConfirmed)
                return new ProcessResult { Message = "Please Confirm Email" };

            return new ProcessResult
            {
                Message = user.Id,
                IsSuccesed = true
            };
        }

        //done
        public async Task<ProcessResult> ResetPasswordAsync(string userId, string newPassword)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(newPassword))
            {
                return new ProcessResult { Message = "Password or UserId cannot be Empty or Null" };
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new ProcessResult { Message = $"Unable to load user with ID '{userId}'." };
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, newPassword);

            var processResult = new ProcessResult();
            processResult.Message = result.Succeeded ? "Password Reset Succesfully" : "Unable to reset password";
            processResult.IsSuccesed = result.Succeeded;

            return processResult;
        }

        //done
        public async Task<ProcessResult> AddRoleAsync(RoleDto model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
                return new ProcessResult { Message = "Invalid username or Role" };

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return new ProcessResult { Message = "User already assigned to this role" };

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);
            var processResult = new ProcessResult();
            processResult.Message = result.Succeeded ? string.Empty : "Something went wrong";
            processResult.IsSuccesed = result.Succeeded ? true : false;

            return processResult;
        }

        //done
        private async Task<JwtSecurityToken> CreateJwtToken(AppUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public string GenerateOTP()
        {
            var random = new Random();
            var code = random.Next(0, 1000000).ToString();
            return code;
        }

    }
}
