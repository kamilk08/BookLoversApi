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
    internal class CycleByIdHandler : IQueryHandler<CycleByIdQuery, PublisherCycleDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public CycleByIdHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PublisherCycleDto> HandleAsync(CycleByIdQuery query)
        {
            var cycle = await this._context.PublisherCycles
                .Include(p => p.Publisher)
                .Include(p => p.CycleBooks)
                .AsNoTracking().ActiveRecords()
                .SingleOrDefaultAsync(p => p.Id == query.Id);

            return this._mapper.Map<PublisherCycleReadModel, PublisherCycleDto>(cycle);
        }
    }
}