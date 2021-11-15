using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration
{
    internal class BookQuoteAddedHandler :
        IIntegrationEventHandler<BookQuoteAddedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookQuoteAddedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(BookQuoteAddedIntegrationEvent @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new AddActivityOfTypeBookQuoteAddedInternalCommand(@event.QuoteGuid, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.AddedQuotes.Value,
                    StatisticStep.Increase.Value));
        }
    }
}