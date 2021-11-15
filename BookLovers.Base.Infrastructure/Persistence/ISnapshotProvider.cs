using System;
using System.Threading.Tasks;

namespace BookLovers.Base.Infrastructure.Persistence
{
    public interface ISnapshotProvider
    {
        Task SaveSnapshotAsync(ISnapshot snapshot);

        Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid);

        Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid, int snapshotVersion);
    }
}