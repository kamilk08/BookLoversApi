using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers
{
    internal class ReaderCreatedEventHandler : IDomainEventHandler<ReaderCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderCreated @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new CreateStatisticsGathererInternalCommand(@event.StatisticsGathererGuid, @event.AggregateGuid,
                    @event.SocialProfileGuid));
        }
    }
}