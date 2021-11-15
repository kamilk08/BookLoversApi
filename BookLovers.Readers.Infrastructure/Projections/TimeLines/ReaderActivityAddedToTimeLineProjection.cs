using System.Data.Entity;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.TimeLine;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Projections.TimeLines
{
    internal class ReaderActivityAddedToTimeLineProjection :
        IProjectionHandler<ActivityAddedToTimeLine>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public ReaderActivityAddedToTimeLineProjection(ReadersContext context)
        {
            this._context = context;
        }

        public void Handle(ActivityAddedToTimeLine @event)
        {
            var timeline = this._context.TimeLines.Include(p => p.Actvities)
                .Single(p => p.Guid == @event.TimeLineGuid);

            timeline.Actvities.Add(new TimeLineActivityReadModel()
            {
                ActivityType = @event.ActivityType,
                ActivityObjectGuid = @event.ActivityObjectGuid,
                Title = @event.Title,
                Date = @event.Date,
                Show = @event.ShowToOthers
            });

            this._context.SaveChanges();
        }
    }
}