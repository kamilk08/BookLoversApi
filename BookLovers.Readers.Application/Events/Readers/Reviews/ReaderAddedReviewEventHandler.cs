using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers.Reviews
{
    internal class ReaderAddedReviewEventHandler :
        IDomainEventHandler<ReaderAddedReview>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderAddedReviewEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderAddedReview @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddReviewActivityInternalCommand(@event.ReviewGuid, @event.AggregateGuid, @event.BookGuid,
                    @event.AddedAt));
        }
    }
}