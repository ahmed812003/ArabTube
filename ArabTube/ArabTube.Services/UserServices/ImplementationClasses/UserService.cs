using ArabTube.Entities.Models;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace ArabTube.Services.UserServices.ImplementationClasses
{
    public class UserService
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
    }
}
