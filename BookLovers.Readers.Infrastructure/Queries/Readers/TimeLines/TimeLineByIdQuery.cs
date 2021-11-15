using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.TimeLines
{
    public class TimeLineByIdQuery : IQuery<TimeLineDto>
    {
        public int TimelineId { get; set; }

        public TimeLineByIdQuery()
        {
        }

        public TimeLineByIdQuery(int timelineId)
        {
            TimelineId = timelineId;
        }
    }
}