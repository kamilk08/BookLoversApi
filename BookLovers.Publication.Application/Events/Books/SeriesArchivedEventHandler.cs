using System;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.SeriesCycle;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class SeriesArchivedEventHandler :
        IDomainEventHandler<SeriesArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public SeriesArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(SeriesArchived @event)
        {
            var tasks = @event.Books.Select(async bookGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(
                    new ChangeBookSeriesInternalCommand(bookGuid, Guid.Empty, null)));

            return Task.WhenAll(tasks);
        }
    }
}