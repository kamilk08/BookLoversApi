using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls;
using BookLovers.Readers.Domain.NotificationWalls.Notifications;
using BookLovers.Readers.Domain.NotificationWalls.Services;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.CommandHandlers.NotificationWalls
{
    internal class AddReviewReportedNotificationHandler :
        ICommandHandler<AddReviewReportedNotificationInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;

        public AddReviewReportedNotificationHandler(
            IUnitOfWork unitOfWork,
            NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task HandleAsync(
            AddReviewReportedNotificationInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);
            var notificationWall = await _unitOfWork.GetAsync<NotificationWall>(
                reader.Socials.NotificationWallGuid);

            var notificationItems = new List<NotificationItem>
            {
                new NotificationItem(command.Guid, NotificationItemType.Review),
                new NotificationItem(command.ReaderGuid, NotificationItemType.User),
                new NotificationItem(command.ReportedByGuid, NotificationItemType.User)
            };

            _notificationService.AddNotification(notificationWall, notificationItems, NotificationSubType.ReviewReported);

            await _unitOfWork.CommitAsync(notificationWall);
        }
    }
}