using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Application.WriteModels.Books;
using Newtonsoft.Json;

namespace BookLovers.Publication.Application.Integration.Books
{
    internal class BookAcceptedByLibrarianHandler :
        IIntegrationEventHandler<BookAcceptedByLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookAcceptedByLibrarianHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookAcceptedByLibrarian @event)
        {
            var writeModel = JsonConvert.DeserializeObject<CreateBookWriteModel>(@event.BookData);

            return this._commandDispatcher.SendInternalCommandAsync<AddBookCommand>(
                new AddBookCommand(writeModel));
        }
    }
}