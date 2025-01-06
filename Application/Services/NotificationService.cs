using Application.DTOs;
using Application.ServiceInterfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.General;
using Domain.Interfaces.Queries.User;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IGlobalNotificationCommandRepository _globalNotificationCommandRepository;
        private readonly IGlobalNotificationQueryRepository _globalNotificationQueryRepository;
        private readonly INotificationCommandRepository _notificationCommandRepository;
        private readonly INotificationQueryRepository _notificationQueryRepository;
        private readonly ILogService _logService;
        private readonly IMapper _mapper;
        public NotificationService(
            IGlobalNotificationCommandRepository globalNotificationCommandRepository,
            IGlobalNotificationQueryRepository globalNotificationQueryRepository,
            INotificationCommandRepository notificationCommandRepository,
            INotificationQueryRepository notificationQueryRepository,
            ILogService logService,
            IMapper mapper
            
        )
        {
            _globalNotificationCommandRepository = globalNotificationCommandRepository;
            _globalNotificationQueryRepository = globalNotificationQueryRepository;
            _notificationCommandRepository = notificationCommandRepository;
            _notificationQueryRepository = notificationQueryRepository;
            _logService = logService;
            _mapper = mapper;
        }

        public async Task DeleteGlobalNotification(Guid notificationId)
        {
            try {
                await _globalNotificationCommandRepository.DeleteGlobalNotificationAsync(notificationId);
                await _logService.CreateLogAsync($"Global notification deleted with id: {notificationId}", LogType.Information,null, null, null);
            }
            catch (Exception e)
            {
                await _logService.CreateLogAsync($"Error while deleting global notification: {e.Message}", LogType.Error, e.StackTrace, null, null);
            }
        }

        public async Task DeleteNotification(Guid notificationId)
        {
            try
            {
                await _notificationCommandRepository.DeleteNotificationAsync(notificationId);
                await _logService.CreateLogAsync($"Notification deleted with id: {notificationId}", LogType.Information,null, null, null);
            }
            catch (Exception e)
            {
                await _logService.CreateLogAsync($"Error while deleting notification: {e.Message}", LogType.Error,e.StackTrace, null, null);
            }
        }

        public async Task<NotificationDto> GetGlobalNotification(Guid notificationId)
        {
            try { 
            var notification = await _globalNotificationQueryRepository.GetGlobalNotificationByIdAsync(notificationId);
            return _mapper.Map<NotificationDto>(notification);
            }
            catch (Exception e)
            {
                await _logService.CreateLogAsync($"Error while getting global notification: {e.Message}", LogType.Error,e.StackTrace, null, null);
            }
            return new NotificationDto();
        }

        public async Task<NotificationDto> GetNotification(Guid notificationId)
        {
            try { 
            var notification =  await _notificationQueryRepository.GetNotificationAsync(notificationId);
            return _mapper.Map<NotificationDto>(notification);
            }
            catch (Exception e)
            {
                await _logService.CreateLogAsync($"Error while getting notification: {e.Message}", LogType.Error,e.StackTrace, null, null);
            }
            return new NotificationDto();
        }

        public async Task<ICollection<NotificationDto>> GetPaginatedUserNotification(Guid userId, int PageSize, int PageNumber)
        {
            try { 
            var notifications = _mapper.Map<ICollection<NotificationDto>>(await _notificationQueryRepository.GetPaginatedUserNotifications(userId, PageSize / 2, PageNumber));
            var globalNotifications = _mapper.Map<ICollection<NotificationDto>>(await _globalNotificationQueryRepository.GetPaginatedGlobalNotifications(PageSize / 2, PageNumber));
            var result = new List<NotificationDto>();
            result.AddRange(notifications);
            result.AddRange(globalNotifications);
            return result.OrderBy(x => x.CreatedAt).ToList();
            }
            catch (Exception e)
            {
                await _logService.CreateLogAsync($"Error while getting paginated user notifications: {e.Message}", LogType.Error,e.StackTrace,null, null);
            }
            return new List<NotificationDto>();
        }

        public async Task ReadNotification(Guid notificationId)
        {
            try
            {
                await _notificationCommandRepository.SetReadNotificationAsync(notificationId, true);
            }
            catch (Exception e)
            {
               await _logService.CreateLogAsync($"Error while reading notification: {e.Message}", LogType.Error,e.StackTrace, null, null);
            }
        }

        public async Task SendNotification(NotificationDto notification)
        {
            try
            {
                if (notification.userId != null)
                {
                    var resultNotification = _mapper.Map<Notification>(notification);
                    await _notificationCommandRepository.CreateNotificationAsync(resultNotification);
                }
                else
                {
                    var resultNotification = _mapper.Map<GlobalNotification>(notification);
                    await _globalNotificationCommandRepository.CreateGlobalNotificationAsync(resultNotification);
                }
            }
            catch (Exception e)
            {
                await _logService.CreateLogAsync($"Error while sending notification: {e.Message}", LogType.Error, null, notification.userId, null);
            }
        }
    }
}
