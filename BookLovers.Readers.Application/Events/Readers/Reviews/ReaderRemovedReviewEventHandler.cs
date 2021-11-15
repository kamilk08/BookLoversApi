using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers.Reviews
{
    internal class ReaderRemovedReviewEventHandler :
        IDomainEventHandler<ReviewRemoved>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderRemovedReviewEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReviewRemoved @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(new RemoveReviewInternalCommand(@event.ReviewGuid));
        }
    }
}