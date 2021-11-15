using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class TimeLineActivitiesTableConfiguration :
        EntityTypeConfiguration<TimeLineActivityReadModel>
    {
        public TimeLineActivitiesTableConfiguration()
        {
            this.ToTable("TimelineActivities");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Title).HasColumnOrder(2).IsRequired().HasColumnName("Title");

            this.Property(p => p.ActivityType).HasColumnOrder(3).IsRequired()
                .HasColumnName("ActivityType");

            this.Property(p => p.Show).IsRequired().HasColumnOrder(4).HasColumnName("Show");

            this.Property(p => p.Date).HasPrecision(1);
        }
    }
}