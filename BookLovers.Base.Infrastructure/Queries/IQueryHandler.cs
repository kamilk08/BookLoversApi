using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Queries
{
    public interface IQueryHandler<TQuery, TResult>
        where TQuery : class, IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}