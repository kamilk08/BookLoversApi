using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Publication.Store.Persistence.Configuration
{
    internal class SnapshotsTableConfiguration : EntityTypeConfiguration<Snapshot>
    {
        public SnapshotsTableConfiguration()
        {
            this.ToTable("Snapshots");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.AggregateGuid).IsRequired().HasColumnOrder(2);

            this.Property(p => p.SnapshottedAggregate).IsRequired().HasColumnOrder(3);

            this.Property(p => p.Version).IsRequired().HasColumnOrder(4);
        }
    }
}