using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class ShelfRecordTrackersConfiguration :
        EntityTypeConfiguration<ShelfRecordTrackerReadModel>
    {
        public ShelfRecordTrackersConfiguration()
        {
            ToTable("ShelfRecordTrackers");

            HasKey(p => p.Id);

            Property(p => p.ShelfRecordTrackerGuid).IsRequired();

            Property(p => p.BookcaseGuid)
                .IsRequired();

            HasMany(p => p.ShelfRecords)
                .WithMany()
                .Map(cfg => cfg.ToTable("ShelfRecordsWithTrackers"));
        }
    }
}