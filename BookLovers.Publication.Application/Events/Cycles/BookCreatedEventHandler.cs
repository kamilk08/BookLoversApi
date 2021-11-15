using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Application.Events.Cycles
{
    internal class BookCreatedEventHandler : IDomainEventHandler<BookCreated>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(BookCreated @event)
        {
            var tasks = @event.Cycles.Select(async cycleGuid =>
                await this._commandDispatcher.SendInternalCommandAsync(
                    new AddPublisherCycleBookInternalCommand(cycleGuid, @event.AggregateGuid, false)));

            return Task.WhenAll(tasks);
        }
    }
}