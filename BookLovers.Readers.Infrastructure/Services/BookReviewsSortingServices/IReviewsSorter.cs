using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;

namespace BookLovers.Readers.Infrastructure.Services.BookReviewsSortingServices
{
    public interface IReviewsSorter
    {
        ReviewsSortingType SortingType { get; }

        Task<PaginatedResult<ReviewDto>> Sort(
            PaginatedBookReviewsQuery query);
    }
}