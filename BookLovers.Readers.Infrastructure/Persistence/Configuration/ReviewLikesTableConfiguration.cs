using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class ReviewLikesTableConfiguration : EntityTypeConfiguration<ReviewLikeReadModel>
    {
        public ReviewLikesTableConfiguration()
        {
            this.ToTable("ReviewLikes");

            this.Property(p => p.ReaderGuid).IsRequired();

            this.Property(p => p.ReaderId).IsRequired();
        }
    }
}