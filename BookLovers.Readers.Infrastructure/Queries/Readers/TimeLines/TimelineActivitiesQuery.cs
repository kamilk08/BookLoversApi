using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;

namespace BookLovers.Readers.Infrastructure.Queries.Readers.TimeLines
{
    public class TimelineActivitiesQuery : IQuery<PaginatedResult<TimeLineActivityDto>>
    {
        public int TimelineId { get; set; }

        public int Page { get; set; }

        public int Count { get; set; }

        public bool Hidden { get; set; }

        public TimelineActivitiesQuery()
        {
            var page = this.Page == 0 ? PaginatedResult.DefaultPage : this.Page;
            var count = this.Count == 0 ? PaginatedResult.DefaultItemsPerPage : this.Count;

            this.Page = page;
            this.Count = count;
        }

        public TimelineActivitiesQuery(int timelineId, int? page, int? count, bool? hidden)
        {
            this.TimelineId = timelineId;

            this.Page = page ?? PaginatedResult.DefaultPage;
            this.Count = count ?? PaginatedResult.DefaultItemsPerPage;
            this.Hidden = hidden.GetValueOrDefault();
        }
    }
}