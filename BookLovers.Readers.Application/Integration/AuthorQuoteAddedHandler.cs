using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Authors;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration
{
    internal class AuthorQuoteAddedHandler :
        IIntegrationEventHandler<AuthorQuoteAddedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorQuoteAddedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(AuthorQuoteAddedIntegrationEvent @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new AddActivityOfTypeAuthorQuoteAddedInternalCommand(@event.ReaderGuid, @event.QuoteGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.AddedQuotes.Value,
                    StatisticStep.Increase.Value));
        }
    }
}