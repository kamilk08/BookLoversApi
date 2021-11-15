﻿using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.Services;

namespace BookLovers.Readers.Application.CommandHandlers.NotificationWalls
{
    internal class AddReaderFollowedNotificationHandler :
        ICommandHandler<AddNewFollowerNotificationInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;

        public AddReaderFollowedNotificationHandler(
            IUnitOfWork unitOfWork,
            NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task HandleAsync(AddNewFollowerNotificationInternalCommand command)
        {
            var notificationWall = await _unitOfWork.GetAsync<NotificationWall>(command.NotificationWallGuid);

            var notificationItems = new List<NotificationItem>
            {
                new NotificationItem(command.Guid, NotificationItemType.User),
                new NotificationItem(command.FollowedByGuid, NotificationItemType.Follower)
            };

            _notificationService.AddNotification(notificationWall, notificationItems,
                NotificationSubType.NewFollower);

            await _unitOfWork.CommitAsync(notificationWall, false);
        }
    }
}