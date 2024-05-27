using ArabTube.Entities.DtoModels.NotificationsDTOs;
using ArabTube.Entities.Models;
using ArabTube.Entities.NotificationModels;
using ArabTube.Services.ControllersServices.NotificationServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;


namespace ArabTube.Services.ControllersServices.NotificationServices.ImplementationClasses
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<GetNotificationResult> NotificationsAsync(string userId)
        {
            var notifications = await _unitOfWork.Notification.GetNotificationsAsync(userId);
            if (!notifications.Any())
            {
                return new GetNotificationResult { Message = "You don't have any notification"};
            }

            var notificationsDto = notifications.Select(n => new GetNotificationsDto
            {
                Message = n.Message,
                SendTime = n.Date,
                ChannelId = n.SenderId,
                UserId = n.User.Id
            }) ;
            return new GetNotificationResult
            {
                IsSuccesed = true,
                NotificationsDtos = notificationsDto
            };
        }
    }
}
