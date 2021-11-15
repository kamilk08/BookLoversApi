using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.IntegrationEvents;
using BookLovers.Librarians.Integration.IntegrationEvents;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.WriteModels.Author;
using Newtonsoft.Json;

namespace BookLovers.Publication.Application.Integration.Authors
{
    internal class AuthorAcceptedByLibrarianHandler :
        IIntegrationEventHandler<AuthorAcceptedByLibrarian>,
        IIntegrationEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AuthorAcceptedByLibrarianHandler(IInternalCommandDispatcher commandDispatcher) =>
            this._commandDispatcher = commandDispatcher;

        public Task HandleAsync(AuthorAcceptedByLibrarian @event)
        {
            var writeModel = JsonConvert.DeserializeObject<CreateAuthorWriteModel>(
                @event.AuthorData);

            writeModel.AuthorWriteModel.ReaderGuid = @event.ReaderGuid;

            return this._commandDispatcher.SendInternalCommandAsync(new CreateAuthorCommand(writeModel));
        }
    }
}