using System;
using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface IEventStore
    {
        Task StoreEvents<TAggregate>(TAggregate aggregate)
            where TAggregate : class;

        Task<TAggregate> GetAsync<TAggregate>(Guid aggregateGuid)
            where TAggregate : class;
    }
}