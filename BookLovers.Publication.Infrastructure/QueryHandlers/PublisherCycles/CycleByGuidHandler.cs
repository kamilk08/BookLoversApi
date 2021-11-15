using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.PublisherCycles
{
    internal class CycleByGuidHandler : IQueryHandler<CycleByGuidQuery, PublisherCycleDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public CycleByGuidHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PublisherCycleDto> HandleAsync(CycleByGuidQuery query)
        {
            var cycle = await this._context.PublisherCycles
                .AsNoTracking().ActiveRecords()
                .Include(p => p.CycleBooks)
                .Include(p => p.Publisher)
                .SingleOrDefaultAsync(p => p.Guid == query.PublisherCycleGuid);

            return this._mapper.Map<PublisherCycleReadModel, PublisherCycleDto>(cycle);
        }
    }
}