using ArabTube.Entities.GenericModels;
using ArabTube.Entities.SubscriptionModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.ControllersServices.SubscriptionServices.Interfaces
{
    public interface ISubscriptionService
    {
        Task<GetFollowingResult> GetFollowingAsync(string userId);
        Task<ProcessResult> SubscribeAsync(string ownerId, string userId);
        Task<ProcessResult> UnScbscribeAsync(string ownerId, string userId);
        Task<ProcessResult> GetNotificationsAsync(string ownerId, string userId);
        Task<ProcessResult> IsUserGetAllNotificationsAsync(string ownerId, string userId);
    }
}
