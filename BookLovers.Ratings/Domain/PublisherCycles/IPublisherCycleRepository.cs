using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Ratings.Domain.PublisherCycles
{
    public interface IPublisherCycleRepository : IRepository<PublisherCycle>
    {
        Task<PublisherCycle> GetByCycleGuidAsync(Guid cycleGuid);

        Task<PublisherCycle> GetByIdAsync(int cycleId);

        Task<IEnumerable<PublisherCycle>> GetCyclesWithBookAsync(
            Guid bookGuid);
    }
}