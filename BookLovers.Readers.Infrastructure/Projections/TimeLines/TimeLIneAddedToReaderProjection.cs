using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.TimeLine;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.TimeLines
{
    internal class TimeLIneAddedToReaderProjection :
        IProjectionHandler<TimeLineAddedToReader>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public TimeLIneAddedToReaderProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(TimeLineAddedToReader @event)
        {
            var reader = this._context.Readers.Single(p => p.Guid == @event.AggregateGuid);

            this._context.TimeLines.Add(new TimeLineReadModel()
            {
                ReaderId = reader.ReaderId,
                Guid = @event.TimeLineGuid,
                Status = @event.TimeLineStatus
            });

            this._context.SaveChanges();
        }
    }
}