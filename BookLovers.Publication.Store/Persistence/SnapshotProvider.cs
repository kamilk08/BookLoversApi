using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Publication.Store.Persistence
{
    public class SnapshotProvider : ISnapshotProvider
    {
        private readonly PublicationsStoreContext _context;

        public SnapshotProvider(PublicationsStoreContext context)
        {
            this._context = context;
        }

        public Task SaveSnapshotAsync(ISnapshot snapshot)
        {
            _context.Snapshots.Add(snapshot as Snapshot);
            _context.SaveChanges();

            return Task.CompletedTask;
        }

        public Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid)
        {
            return _context.Snapshots
                .Where<ISnapshot>(p => p.AggregateGuid == aggregateGuid)
                .OrderByDescending(p => p.Version).FirstOrDefaultAsync();
        }

        public Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid, int snapshotVersion)
        {
            return _context.Snapshots.AsNoTracking()
                .SingleOrDefaultAsync<ISnapshot>(
                    p => p.AggregateGuid == aggregateGuid && p.Version == snapshotVersion);
        }
    }
}