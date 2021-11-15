using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class ReadersTableConfiguration : EntityTypeConfiguration<ReaderReadModel>
    {
        public ReadersTableConfiguration()
        {
            this.ToTable("Readers");

            this.HasKey(p => p.Id);

            this.Property(p => p.ReaderGuid).IsRequired();

            this.Property(p => p.ReaderId).IsRequired();

            this.HasIndex(p => p.ReaderId).IsUnique();

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.Status).IsRequired();
        }
    }
}