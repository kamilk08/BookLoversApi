using System;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Aggregates;

namespace BookLovers.Base.Infrastructure
{
    public interface IRepository<TAggregate>
        where TAggregate : class, IRoot
    {
        Task<TAggregate> GetAsync(Guid aggregateGuid);

        Task CommitChangesAsync(TAggregate aggregate);
    }
}