using ArabTube.Entities.DtoModels.UserConnectionsDto;
using ArabTube.Entities.GenericModels;
using ArabTube.Entities.Models;
using ArabTube.Entities.SubscriptionModels;
using ArabTube.Services.ControllersServices.SubscriptionServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.ControllersServices.SubscriptionServices.ImplementationClasses
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public SubscriptionService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<GetFollowingResult> GetFollowingAsync(string userId)
        {
            var appUserConnections = await _unitOfWork.AppUserConnection.GetFollowingAsync(userId);
            
            if (appUserConnections == null || !appUserConnections.Any())
            {
                return new GetFollowingResult { Message = "The Current Login User Has No Following" };
            }

            var following = appUserConnections.Select(auc => new FollowingDto
            {
                Username = auc.Following.UserName
            });

            return new GetFollowingResult
            {
                IsSuccesed = true,
                Followings = following
            };
        }
    
        public async Task<ProcessResult> SubscribeAsync(string ownerId , string userId)
        {
            var owner = await _userManager.FindByIdAsync(ownerId);
            if (owner == null)
            {
                return new ProcessResult { Message = $"No User With Id = {ownerId}" };
            }
                
            var result = await _unitOfWork.AppUserConnection.SubscribeAsync(ownerId, userId);
            if (!result)
            {
                return new ProcessResult { IsSuccesed = true, Message = "You Already Subscriber To This User" };
            }

            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true, Message = "Subscribe Succesfully" };
        }

        public async Task<ProcessResult> UnScbscribeAsync(string ownerId, string userId)
        {
            var owner = await _userManager.FindByIdAsync(ownerId);
            if (owner == null)
            {
                return new ProcessResult { Message = $"No User With Id = {ownerId}" };
            }

            var result = await _unitOfWork.AppUserConnection.UnSubscribeAsync(ownerId, userId);
            if (!result)
            {
                return new ProcessResult { Message = $"You Already Not A Follower To User With Id = {ownerId}" };
            }

            await _unitOfWork.Complete();
            return new ProcessResult { IsSuccesed = true };
        }

    }
}
