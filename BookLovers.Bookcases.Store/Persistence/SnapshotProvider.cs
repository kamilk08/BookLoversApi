using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Persistence;

namespace BookLovers.Bookcases.Store.Persistence
{
    public class SnapshotProvider : ISnapshotProvider
    {
        private readonly BookcaseStoreContext _context;

        public SnapshotProvider(BookcaseStoreContext context)
        {
            _context = context;
        }

        public async Task SaveSnapshotAsync(ISnapshot snapshot)
        {
            _context.Snapshots.Add((Snapshot) snapshot);

            await _context.SaveChangesAsync();
        }

        public Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid)
        {
            return _context.Snapshots
                .Where<ISnapshot>(p => p.AggregateGuid == aggregateGuid)
                .OrderByDescending(p => p.Version).FirstOrDefaultAsync();
        }

        public Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid, int snapshotVersion)
        {
            return _context.Snapshots
                .AsNoTracking()
                .SingleOrDefaultAsync<ISnapshot>(p =>
                    p.AggregateGuid == aggregateGuid
                    && p.Version == snapshotVersion);
        }
    }
}