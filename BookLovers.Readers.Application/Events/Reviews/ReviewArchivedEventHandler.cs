using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Reviews;

namespace BookLovers.Readers.Application.Events.Reviews
{
    internal class ReviewArchivedEventHandler :
        IDomainEventHandler<ReviewArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReviewArchived @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new RemoveReviewResourceInternalCommand(@event.ReaderGuid, @event.AggregateGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.Reviews.Value,
                    StatisticStep.Decrease.Value));
        }
    }
}