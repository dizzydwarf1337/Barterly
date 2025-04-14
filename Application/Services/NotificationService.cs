using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Commands.General;
using Domain.Interfaces.Commands.User;
using Domain.Interfaces.Queries.General;
using Domain.Interfaces.Queries.User;
using MediatR;
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
            await _globalNotificationCommandRepository.DeleteGlobalNotificationAsync(notificationId);
            await _logService.CreateLogAsync($"Global notification deleted with id: {notificationId}", LogType.Information,null, null, null);
        }

        public async Task DeleteNotification(Guid notificationId)
        {
            await _notificationCommandRepository.DeleteNotificationAsync(notificationId);
            await _logService.CreateLogAsync($"Notification deleted with id: {notificationId}", LogType.Information,null, null, null);
        }

        public async Task<NotificationDto> GetGlobalNotification(Guid notificationId)
        {
            var notification = await _globalNotificationQueryRepository.GetGlobalNotificationByIdAsync(notificationId);
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<NotificationDto> GetNotification(Guid notificationId)
        {
            var notification =  await _notificationQueryRepository.GetNotificationAsync(notificationId);
            return _mapper.Map<NotificationDto>(notification);
        }

        public async Task<ICollection<NotificationDto>> GetPaginatedUserNotification(Guid userId, int PageSize, int PageNumber)
        {
            var notifications = _mapper.Map<ICollection<NotificationDto>>(await _notificationQueryRepository.GetPaginatedUserNotifications(userId, PageSize / 2, PageNumber));
            var globalNotifications = _mapper.Map<ICollection<NotificationDto>>(await _globalNotificationQueryRepository.GetPaginatedGlobalNotifications(PageSize / 2, PageNumber));
            var result = new List<NotificationDto>();
            result.AddRange(notifications);
            result.AddRange(globalNotifications);
            return result.OrderBy(x => x.CreatedAt).ToList();
        }

        public async Task ReadNotification(Guid notificationId)
        {
            await _notificationCommandRepository.SetReadNotificationAsync(notificationId, true);   
        }

        public async Task SendGlobalNotification(NotificationDto globalNotification)
        {
            var resultNotification = _mapper.Map<GlobalNotification>(globalNotification);
            await _globalNotificationCommandRepository.CreateGlobalNotificationAsync(resultNotification);
        }

        public async Task SendNotification(Notification notification)
        {
            await _notificationCommandRepository.CreateNotificationAsync(notification);
        }
    }
}
