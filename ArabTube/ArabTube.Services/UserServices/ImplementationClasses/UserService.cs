using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using ArabTube.Services.UserServices.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ArabTube.Services.UserServices.ImplementationClasses
{
    public class UserService:IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<string>> GetChannelsName(string query)
        {
            return await _unitOfWork.AppUser.SearchChannelsAsync(query);
        }

        public async Task<IEnumerable<UserViewDto>> GetUsersAsync(string query)
        {
            var users = await _unitOfWork.AppUser.SearchUsersAsync(query);
            IEnumerable<UserViewDto> userViewDtoList = _mapper.Map<IEnumerable<UserViewDto>>(users);
            return userViewDtoList;
        }
    }
}
