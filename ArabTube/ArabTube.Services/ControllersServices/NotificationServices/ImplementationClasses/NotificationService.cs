using ArabTube.Entities.DtoModels.NotificationsDTOs;
using ArabTube.Entities.NotificationModels;
using ArabTube.Services.ControllersServices.NotificationServices.Interfaces;
using ArabTube.Services.DataServices.Repositories.Interfaces;
using AutoMapper;


namespace ArabTube.Services.ControllersServices.NotificationServices.ImplementationClasses
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                ChannelTitle = $"{n.User.FirstName} {n.User.LastName}",
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
