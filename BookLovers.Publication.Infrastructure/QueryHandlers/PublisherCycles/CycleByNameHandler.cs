using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Persistence;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;
using BookLovers.Publication.Infrastructure.Queries.QueryFilters.Extensions;

namespace BookLovers.Publication.Infrastructure.QueryHandlers.PublisherCycles
{
    internal class CycleByNameHandler : IQueryHandler<CycleByNameQuery, PublisherCycleDto>
    {
        private readonly PublicationsContext _context;
        private readonly IMapper _mapper;

        public CycleByNameHandler(PublicationsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PublisherCycleDto> HandleAsync(CycleByNameQuery query)
        {
            var cycle = await this._context.PublisherCycles
                .AsNoTracking().Include(p => p.CycleBooks)
                .FindCycleWithExactName(query.Name);

            return this._mapper.Map<PublisherCycleReadModel, PublisherCycleDto>(cycle);
        }
    }
}