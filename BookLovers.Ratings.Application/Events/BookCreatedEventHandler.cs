using System;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Events;

namespace BookLovers.Ratings.Application.Events
{
    internal class BookCreatedEventHandler : IDomainEventHandler<BookCreatedEvent>, IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookCreatedEventHandler(
            IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(BookCreatedEvent @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new AddPublisherBookInternalCommand(@event.PublisherGuid, @event.BookGuid));

            if (@event.SeriesGuid != Guid.Empty)
                await this._commandDispatcher.SendInternalCommandAsync(
                    new AddSeriesBookInternalCommand(@event.SeriesGuid, @event.BookGuid));

            foreach (var cycle in @event.Cycles)
                await this._commandDispatcher.SendInternalCommandAsync(
                    new AddPublisherCycleBookInternalCommand(cycle, @event.BookGuid));
        }
    }
}