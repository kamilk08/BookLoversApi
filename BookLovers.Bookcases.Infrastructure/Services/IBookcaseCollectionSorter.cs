using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;

namespace BookLovers.Bookcases.Infrastructure.Services
{
    internal interface IBookcaseCollectionSorter
    {
        BookcaseCollectionSortType SortType { get; }

        Task<PaginatedResult<int>> Sort(PaginatedBookcaseCollectionQuery query);
    }
}