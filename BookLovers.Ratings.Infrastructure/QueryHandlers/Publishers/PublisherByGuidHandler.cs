using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Publishers;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Publishers
{
    internal class PublisherByGuidHandler : IQueryHandler<PublisherByGuidQuery, PublisherDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public PublisherByGuidHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<PublisherDto> HandleAsync(PublisherByGuidQuery query)
        {
            var publisher = await this._context.Publishers
                .AsNoTracking()
                .Include(p => p.Books)
                .Include(p => p.PublisherCycles)
                .FirstOrDefaultAsync(p => p.PublisherGuid == query.PublisherGuid);

            return this._mapper.Map<PublisherReadModel, PublisherDto>(publisher);
        }
    }
}