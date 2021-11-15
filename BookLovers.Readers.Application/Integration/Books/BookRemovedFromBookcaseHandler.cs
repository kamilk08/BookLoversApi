using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Bookcases.Integration.IntegrationEvents;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration.Books
{
    internal class BookRemovedFromBookcaseHandler :
        IIntegrationEventHandler<BookRemovedFromBookcaseIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRemovedFromBookcaseHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookRemovedFromBookcaseIntegrationEvent @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(new UpdateStatisticsGathererInternalCommand(
                @event.ReaderGuid,
                StatisticType.BooksInBookcase.Value, StatisticStep.Decrease.Value));
        }
    }
}