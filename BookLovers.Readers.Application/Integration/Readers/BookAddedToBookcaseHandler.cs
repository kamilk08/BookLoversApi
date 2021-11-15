using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration.Readers
{
    internal class BookAddedToBookcaseHandler :
        IIntegrationEventHandler<BookAddedToBookcaseIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookAddedToBookcaseHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(BookAddedToBookcaseIntegrationEvent @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new AddBookToBookcaseActivityInternalCommand(@event.ReaderGuid, @event.BookGuid, @event.OccuredOn));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.BooksInBookcase.Value,
                    StatisticStep.Increase.Value));
        }
    }
}