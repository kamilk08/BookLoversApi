using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Statistics
{
    internal class ReaderStatisticsHandler : IQueryHandler<ReaderStatisticsQuery, ReaderStatisticsDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderStatisticsHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ReaderStatisticsDto> HandleAsync(
            ReaderStatisticsQuery query)
        {
            var statistics = await this._context.Statistics
                .Include(p => p.Reader)
                .SingleOrDefaultAsync(p => p.Reader.ReaderId == query.ReaderId);

            return this._mapper.Map<StatisticsReadModel, ReaderStatisticsDto>(statistics);
        }
    }
}