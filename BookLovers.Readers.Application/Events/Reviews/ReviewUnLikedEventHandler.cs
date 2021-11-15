using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Reviews;

namespace BookLovers.Readers.Application.Events.Reviews
{
    internal class ReviewUnLikedEventHandler : IDomainEventHandler<ReviewUnLiked>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReviewUnLikedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReviewUnLiked @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.LikeGiverGuid, StatisticType.GivenLikes.Value,
                    StatisticStep.Decrease.Value));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.LikeReceiverGuid, StatisticType.ReceivedLikes.Value,
                    StatisticStep.Decrease.Value));
        }
    }
}