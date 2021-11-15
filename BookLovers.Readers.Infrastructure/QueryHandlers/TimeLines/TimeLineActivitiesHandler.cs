using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers.TimeLines;
using Z.EntityFramework.Plus;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.TimeLines
{
    internal class TimeLineActivitiesHandler :
        IQueryHandler<TimelineActivitiesQuery, PaginatedResult<TimeLineActivityDto>>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public TimeLineActivitiesHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PaginatedResult<TimeLineActivityDto>> HandleAsync(
            TimelineActivitiesQuery query)
        {
            var baseQuery = this._context.TimeLines.AsNoTracking()
                .ActiveRecords().WithId(query.TimelineId)
                .SelectMany(sm => sm.Actvities)
                .Where(p => p.Show || p.Show != query.Hidden)
                .Where(p =>
                    p.ActivityType == ActivityType.NewFollower.Value ||
                    p.ActivityType == ActivityType.AddedBook.Value ||
                    p.ActivityType == ActivityType.LostFollower.Value ||
                    p.ActivityType == ActivityType.NewBookInBookCase.Value ||
                    p.ActivityType == ActivityType.NewReview.Value);

            var totalCountQuery = baseQuery.DeferredCount();
            var activitiesQuery = baseQuery.OrderByDescending(p => p.Date)
                .Paginate(query.Page, query.Count).Future();

            var totalCount = await totalCountQuery.ExecuteAsync();
            var results = await activitiesQuery.ToListAsync();

            var mappedResults = this._mapper.Map<List<TimeLineActivityReadModel>, List<TimeLineActivityDto>>(results);

            var paginatedResult =
                new PaginatedResult<TimeLineActivityDto>(mappedResults, query.Page, query.Count, totalCount);

            return paginatedResult;
        }
    }
}