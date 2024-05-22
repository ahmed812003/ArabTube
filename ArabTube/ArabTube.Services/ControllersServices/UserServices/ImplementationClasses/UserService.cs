using ArabTube.Entities.DtoModels.UserDTOs;
using ArabTube.Entities.Models;
using ArabTube.Entities.UserModels;
using ArabTube.Services.ControllersServices.UserServices.ImplementationClasses;
using ArabTube.Services.ControllersServices.UserServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using static Azure.Core.HttpHeader;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ArabTube.Services.ControllersServices.UserServices.ImplementationClasses
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<GetChannelsNameResult> GetChannelsNameAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new GetChannelsNameResult { Message = "Query Cannot Be Empty" };
            }

            var names = await _unitOfWork.AppUser.SearchChannelsAsync(query);

            if (!names.Any())
            {
                return new GetChannelsNameResult { Message = "No name Found Matching The Search Query" };
            }

            return new GetChannelsNameResult { IsSuccesed = true, names = names };

        }

        public async Task<GetUsersResult> GetUsersAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new GetUsersResult { Message = "Query Cannot Be Empty" };
            }

            var users = await _unitOfWork.AppUser.SearchUsersAsync(query);
            
            if (!users.Any())
            {
                return new GetUsersResult { Message = "No users Found Matching The Search Query" };
            }

            IEnumerable<UserViewDto> userViewDtoList = _mapper.Map<IEnumerable<UserViewDto>>(users);
            return new GetUsersResult { IsSuccesed = true , users = userViewDtoList};
        }
    }
}
