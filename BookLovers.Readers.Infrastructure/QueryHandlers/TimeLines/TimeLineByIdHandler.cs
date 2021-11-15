using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers.TimeLines;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.TimeLines
{
    internal class TimeLineByIdHandler : IQueryHandler<TimeLineByIdQuery, TimeLineDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public TimeLineByIdHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<TimeLineDto> HandleAsync(TimeLineByIdQuery query)
        {
            var timeline = await this._context.TimeLines.AsNoTracking()
                .ActiveRecords()
                .Include(p => p.Actvities)
                .SingleOrDefaultAsync(
                    p => p.Id == query.TimelineId);

            return this._mapper.Map<TimeLineReadModel, TimeLineDto>(timeline);
        }
    }
}