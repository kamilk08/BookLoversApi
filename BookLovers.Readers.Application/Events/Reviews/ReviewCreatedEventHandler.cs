using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Reviews;

namespace BookLovers.Readers.Application.Events.Reviews
{
    internal class ReviewCreatedEventHandler : IDomainEventHandler<ReviewCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReviewCreated @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(new AddReviewResourceInternalCommand(
                @event.ReaderGuid,
                @event.AggregateGuid, @event.BookGuid, @event.CreatedAt));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.Reviews.Value,
                    StatisticStep.Increase.Value));
        }
    }
}