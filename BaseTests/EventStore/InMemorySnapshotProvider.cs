using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure.Persistence;

namespace BaseTests.EventStore
{
    internal class InMemorySnapshotProvider : ISnapshotProvider
    {
        private List<ISnapshot> _snapshots = new List<ISnapshot>();

        public InMemorySnapshotProvider()
        {
        }

        public Task SaveSnapshotAsync(ISnapshot snapshot)
        {
            _snapshots.Add(snapshot as InMemorySnapshot);

            return Task.CompletedTask;
        }

        public Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid)
        {
            var task = _snapshots.Where<ISnapshot>(p => p.AggregateGuid == aggregateGuid)
                .OrderByDescending(p => p.Version).FirstOrDefault();

            return Task.FromResult(task);
        }

        public Task<ISnapshot> GetSnapshotAsync(Guid aggregateGuid, int snapshotVersion)
        {
            var task = _snapshots
                .SingleOrDefault<ISnapshot>(p =>
                    p.AggregateGuid == aggregateGuid && p.Version == snapshotVersion);

            return Task.FromResult(task);
        }
    }
}