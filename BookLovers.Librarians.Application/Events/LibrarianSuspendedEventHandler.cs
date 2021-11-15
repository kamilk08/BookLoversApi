using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Application.Commands.PromotionWaiters;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Events.Librarians;

namespace BookLovers.Librarians.Application.Events
{
    internal class LibrarianSuspendedEventHandler :
        IDomainEventHandler<LibrarianSuspended>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public LibrarianSuspendedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(LibrarianSuspended @event)
        {
            return this._commandDispatcher
                .SendInternalCommandAsync(new ChangePromotionWaiterStatusInternalCommand(
                    @event.ReaderGuid,
                    PromotionAvailability.UnAvailable.Value));
        }
    }
}