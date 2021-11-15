using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Application.Events.Readers.Authors
{
    internal class ReaderAddedAuthorEventHandler :
        IDomainEventHandler<ReaderAddedAuthor>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public ReaderAddedAuthorEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(ReaderAddedAuthor @event)
        {
            return _commandDispatcher.SendInternalCommandAsync(
                new AddAuthorActivityInternalCommand(@event.AuthorGuid, @event.AggregateGuid, @event.AddedAt));
        }
    }
}