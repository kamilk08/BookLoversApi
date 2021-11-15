using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration
{
    internal class AuthorAddedHandler :
        IIntegrationEventHandler<NewAuthorAddedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorAddedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(NewAuthorAddedIntegrationEvent @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new CreateFavouriteInternalCommand(@event.AuthorGuid, @event.AuthorId, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(new AddAuthorResourceInternalCommand(
                @event.ReaderGuid,
                @event.AuthorGuid, @event.AuthorId, @event.OccuredOn));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.AddedAuthors.Value,
                    StatisticStep.Increase.Value));
        }
    }
}