using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class TimeLinesTableConfiguration : EntityTypeConfiguration<TimeLineReadModel>
    {
        public TimeLinesTableConfiguration()
        {
            this.ToTable("TimeLines");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).HasColumnOrder(2).IsRequired();

            this.Property(p => p.ReaderId).HasColumnOrder(3);

            this.Property(p => p.Status).IsRequired().HasColumnOrder(4);

            this.HasMany(p => p.Actvities).WithRequired()
                .Map(cfg => cfg.MapKey("TimelineId"));
        }
    }
}