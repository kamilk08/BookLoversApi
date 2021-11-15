using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class ReviewEditTableConfiguration : EntityTypeConfiguration<ReviewEditReadModel>
    {
        public ReviewEditTableConfiguration()
        {
            this.ToTable("ReviewEdits");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ReviewGuid).IsRequired().HasColumnOrder(2);

            this.Property(p => p.ReviewId).IsRequired().HasColumnOrder(3);

            this.Property(p => p.Review).HasColumnOrder(4);

            this.Property(p => p.EditedAt).IsRequired().HasColumnOrder(5);
        }
    }
}