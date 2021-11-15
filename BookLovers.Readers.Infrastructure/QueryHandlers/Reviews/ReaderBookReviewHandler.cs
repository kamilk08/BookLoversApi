using System.Data.Entity;
using System.Threading.Tasks;
using AutoMapper;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Persistence;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;

namespace BookLovers.Readers.Infrastructure.QueryHandlers.Reviews
{
    internal class ReaderBookReviewHandler : IQueryHandler<ReaderBookReviewQuery, ReviewDto>
    {
        private readonly ReadersContext _context;
        private readonly IMapper _mapper;

        public ReaderBookReviewHandler(ReadersContext context, IMapper mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }

        public async Task<ReviewDto> HandleAsync(ReaderBookReviewQuery query)
        {
            var review = await this._context.Reviews.Include(p => p.Reader)
                .Include(p => p.Book)
                .Include(p => p.Likes)
                .Include(p => p.ReviewReports)
                .Include(p => p.SpoilerTags)
                .AsNoTracking()
                .ActiveRecords()
                .SingleOrDefaultAsync(p => p.Reader.ReaderId == query.ReaderId
                                           && p.Book.BookId == query.BookId);

            return this._mapper.Map<ReviewReadModel, ReviewDto>(review);
        }
    }
}