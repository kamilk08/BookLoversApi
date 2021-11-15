using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using BookLovers.Base.Infrastructure.Events.DomainEvents;
using BookLovers.Readers.Events.TimeLine;
using BookLovers.Readers.Infrastructure.Persistence;

namespace BookLovers.Readers.Infrastructure.Projections.TimeLines
{
    internal class TimeLineActivityShownProjection :
        IProjectionHandler<ActivityShownOnTimeLine>,
        IProjectionHandler
    {
        private readonly ReadersContext _context;

        public TimeLineActivityShownProjection(ReadersContext context) => this._context = context;

        public void Handle(ActivityShownOnTimeLine @event)
        {
            var activityReadModel = this._context.TimeLineActivities
                .Where(p => p.ActivityObjectGuid == @event.TimeLineObjectGuid &&
                            DbFunctions.TruncateTime(p.Date) == DbFunctions.TruncateTime(@event.OccuredAt)).ToList()
                .OrderBy(p => Math.Abs((@event.OccuredAt - p.Date).TotalMilliseconds))
                .First();

            activityReadModel.Show = true;

            this._context.TimeLineActivities.AddOrUpdate(p => p.Id, activityReadModel);

            this._context.SaveChanges();
        }
    }
}