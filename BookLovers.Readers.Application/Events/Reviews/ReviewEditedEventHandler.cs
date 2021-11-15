using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Reviews;

namespace BookLovers.Readers.Application.Events.Reviews
{
    internal class ReviewEditedEventHandler : IDomainEventHandler<ReviewEdited>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewEditedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReviewEdited @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new EditReviewActivityInternalCommand(@event.AggregateGuid, @event.ReaderGuid, @event.EditedAt));
        }
    }
}