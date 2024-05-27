using ArabTube.Entities.NotificationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArabTube.Services.ControllersServices.NotificationServices.Interfaces
{
    public interface INotificationService
    {
        Task<GetNotificationResult> NotificationsAsync(string userId);
    }
}
