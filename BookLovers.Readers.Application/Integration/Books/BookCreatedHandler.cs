using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Publication.Integration.ApplicationEvents.Books;
using BookLovers.Readers.Application.Commands.Favourites;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.Integration.Books
{
    internal class BookCreatedHandler :
        IIntegrationEventHandler<BookCreatedIntegrationEvent>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(BookCreatedIntegrationEvent @event)
        {
            await _commandDispatcher.SendInternalCommandAsync(
                new CreateFavouriteInternalCommand(@event.BookGuid, @event.BookId, @event.ReaderGuid));

            await _commandDispatcher.SendInternalCommandAsync(new AddBookResourceInternalCommand(
                @event.ReaderGuid,
                @event.BookGuid, @event.BookId, @event.OccuredOn));

            await _commandDispatcher.SendInternalCommandAsync(
                new UpdateStatisticsGathererInternalCommand(@event.ReaderGuid, StatisticType.AddedBooks.Value,
                    StatisticStep.Increase.Value));
        }
    }
}