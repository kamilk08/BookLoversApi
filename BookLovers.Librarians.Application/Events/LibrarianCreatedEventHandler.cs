using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Librarians.Application.Commands.PromotionWaiters;
using BookLovers.Librarians.Domain.PromotionWaiters;
using BookLovers.Librarians.Events.Librarians;

namespace BookLovers.Librarians.Application.Events
{
    internal class LibrarianCreatedEventHandler :
        IDomainEventHandler<LibrarianCreated>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public LibrarianCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(LibrarianCreated @event)
        {
            var librarianCreated =
                new ChangePromotionWaiterStatusInternalCommand(@event.ReaderGuid, PromotionAvailability.Promoted.Value);

            return this._commandDispatcher
                .SendInternalCommandAsync(librarianCreated);
        }
    }
}