using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Persistence;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;

namespace BookLovers.Ratings.Infrastructure.QueryHandlers.Ratings
{
    internal class ReaderByIdHandler : IQueryHandler<ReaderByIdQuery, ReaderDto>
    {
        private readonly RatingsContext _context;
        private readonly IMapper _mapper;

        public ReaderByIdHandler(RatingsContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ReaderDto> HandleAsync(ReaderByIdQuery query)
        {
            var reader = await this._context.Readers.AsNoTracking()
                .SingleOrDefaultAsync(p => p.ReaderId == query.ReaderId);

            return this._mapper.Map<ReaderReadModel, ReaderDto>(reader);
        }
    }
}