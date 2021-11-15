using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers
{
    internal class ReaderUnFollowedEventHandler :
        IDomainEventHandler<ReaderUnFollowed>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderUnFollowedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(ReaderUnFollowed @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.AggregateGuid, StatisticType.Followers.Value,
                    StatisticStep.Decrease.Value));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.UnFollowedByGuid, StatisticType.Followings.Value,
                    StatisticStep.Decrease.Value));
        }
    }
}