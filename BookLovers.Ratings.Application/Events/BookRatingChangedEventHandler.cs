using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Ratings.Application.Commands.Authors;
using BookLovers.Ratings.Application.Commands.BookSeries;
using BookLovers.Ratings.Application.Commands.PublisherCycles;
using BookLovers.Ratings.Application.Commands.Publishers;
using BookLovers.Ratings.Events;

namespace BookLovers.Ratings.Application.Events
{
    internal class BookRatingChangedEventHandler :
        IDomainEventHandler<BookRatingChanged>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public BookRatingChangedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public async Task HandleAsync(BookRatingChanged @event)
        {
            await this._commandDispatcher.SendInternalCommandAsync(
                new RecalculateAuthorAverageInternalCommand(@event.AggregateGuid));

            await this._commandDispatcher.SendInternalCommandAsync(
                new RecalculateSeriesAverageInternalCommand(@event.AggregateGuid));

            await this._commandDispatcher.SendInternalCommandAsync(
                new RecalculateCyclesAverageInternalCommand(@event.AggregateGuid));

            await this._commandDispatcher.SendInternalCommandAsync(
                new RecalculatePublisherAverageInternalCommand(@event.AggregateGuid));
        }
    }
}