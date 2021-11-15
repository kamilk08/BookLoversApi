using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Bookcases.Store.Persistence.Configuration
{
    public class SnapshotsTableConfiguration : EntityTypeConfiguration<Snapshot>
    {
        public SnapshotsTableConfiguration()
        {
            ToTable("Snapshots");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.AggregateGuid).IsRequired().HasColumnOrder(2);

            Property(p => p.SnapshottedAggregate).IsRequired().HasColumnOrder(3);

            Property(p => p.Version).IsRequired().HasColumnOrder(4);
        }
    }
}