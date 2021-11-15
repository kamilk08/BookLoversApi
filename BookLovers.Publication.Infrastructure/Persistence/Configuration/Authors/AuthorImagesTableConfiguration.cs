using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Authors
{
    internal class AuthorImagesTableConfiguration : EntityTypeConfiguration<AuthorImageReadModel>
    {
        public AuthorImagesTableConfiguration()
        {
            this.ToTable("AuthorImages");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.AuthorPictureUrl).HasColumnOrder(2).IsRequired();

            this.Property(p => p.FileName).HasColumnOrder(3).IsRequired();

            this.Property(p => p.MimeType).HasColumnOrder(4).IsRequired();

            this.Property(p => p.AuthorGuid).IsRequired();
        }
    }
}