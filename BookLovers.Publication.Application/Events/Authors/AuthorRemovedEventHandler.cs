using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Authors
{
    internal class AuthorRemovedEventHandler : IDomainEventHandler<AuthorRemoved>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;
        private readonly IUnknownAuthorService _unknownAuthorService;

        public AuthorRemovedEventHandler(
            IInternalCommandDispatcher commandDispatcher,
            IUnknownAuthorService unknownAuthorService)
        {
            this._commandDispatcher = commandDispatcher;
            this._unknownAuthorService = unknownAuthorService;
        }

        public async Task HandleAsync(AuthorRemoved @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new RemoveAuthorBookInternalCommand(@event.AuthorGuid, @event.AggregateGuid));

            if (!@event.AddToUnknownAuthor)
                return;

            var unknownAuthor = await this._unknownAuthorService.GetUnknownAuthorAsync();

            await this._commandDispatcher.SendInternalCommandAsync(
                new AddAuthorBookInternalCommand(unknownAuthor.Guid, @event.AggregateGuid, false));

            await this._commandDispatcher.SendInternalCommandAsync(
                new AddBookAuthorInternalCommand(@event.AggregateGuid, unknownAuthor.Guid));
        }
    }
}