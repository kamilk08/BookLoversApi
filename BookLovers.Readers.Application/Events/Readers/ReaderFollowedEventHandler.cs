using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers
{
    internal class ReaderFollowedEventHandler :
        IDomainEventHandler<ReaderFollowed>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderFollowedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReaderFollowed @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.AggregateGuid, StatisticType.Followers.Value,
                    StatisticStep.Increase.Value));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.FollowedByGuid, StatisticType.Followings.Value,
                    StatisticStep.Increase.Value));
        }
    }
}