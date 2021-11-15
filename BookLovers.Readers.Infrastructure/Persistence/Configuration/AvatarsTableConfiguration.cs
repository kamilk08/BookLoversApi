using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class AvatarsTableConfiguration : EntityTypeConfiguration<AvatarReadModel>
    {
        public AvatarsTableConfiguration()
        {
            this.ToTable("Avatars");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ReaderId).HasColumnName("ReaderId")
                .HasColumnOrder(2);

            this.Property(p => p.AvatarUrl).HasColumnOrder(3).IsRequired();

            this.Property(p => p.FileName).IsRequired().HasColumnOrder(4);

            this.Property(p => p.MimeType).IsRequired().HasColumnOrder(5);
        }
    }
}