using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Ratings.Application.Commands.RatingGivers;
using BookLovers.Ratings.Events;

namespace BookLovers.Ratings.Application.Events
{
    internal class RatingGiverArchivedEventHandler :
        IDomainEventHandler<RatingGiverArchived>,
        IDomainEventHandler
    {
        private readonly IInternalCommandDispatcher _commandDispatcher;

        public RatingGiverArchivedEventHandler(IInternalCommandDispatcher commandDispatcher)
        {
            this._commandDispatcher = commandDispatcher;
        }

        public Task HandleAsync(RatingGiverArchived @event)
        {
            var command = new RemoveAllRatingsInternalCommand(@event.AggregateGuid, @event.ReaderId, @event.BookIds);

            return this._commandDispatcher.SendInternalCommandAsync(command);
        }
    }
}