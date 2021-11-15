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
    internal class AddBookDismissedNotificationHandler :
        ICommandHandler<AddBookDismissedNotificationInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly NotificationService _notificationService;

        public AddBookDismissedNotificationHandler(
            IUnitOfWork unitOfWork,
            NotificationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
        }

        public async Task HandleAsync(
            AddBookDismissedNotificationInternalCommand internalCommand)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(internalCommand.ReaderGuid);
            var notificationWall =
                await _unitOfWork.GetAsync<NotificationWall>(reader.Socials.NotificationWallGuid);

            var notificationItems = new List<NotificationItem>
            {
                new NotificationItem(internalCommand.ReaderGuid, NotificationItemType.User),
                new NotificationItem(internalCommand.DismissedByGuid, NotificationItemType.Librarian),
                new NotificationItem(internalCommand.BookGuid, NotificationItemType.Book)
            };

            _notificationService.AddNotification(notificationWall, notificationItems,
                NotificationSubType.BookDismissedByLibrarian);

            await _unitOfWork.CommitAsync(notificationWall);
        }
    }
}