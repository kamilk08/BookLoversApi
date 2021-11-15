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
    internal class PublisherRatingsHandler : IQueryHandler<PublisherRatingsQuery, RatingsDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public PublisherRatingsHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<RatingsDto> HandleAsync(PublisherRatingsQuery query)
        {
            var ratings = await this._context.Publishers.AsNoTracking()
                .SingleOrDefaultAsync(p => p.PublisherId == query.PublisherId);

            return this._mapper.Map<PublisherReadModel, RatingsDto>(ratings);
        }
    }
}