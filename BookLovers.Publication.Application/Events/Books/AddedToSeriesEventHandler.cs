using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Events.SeriesCycle;

namespace BookLovers.Publication.Application.Events.Books
{
    internal class AddedToSeriesEventHandler : IDomainEventHandler<AddedToSeries>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public AddedToSeriesEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(AddedToSeries @event)
        {
            return this._commandDispatcher.SendInternalCommandAsync(new ChangeBookSeriesInternalCommand(
                @event.BookGuid,
                @event.AggregateGuid, @event.Position));
        }
    }
}