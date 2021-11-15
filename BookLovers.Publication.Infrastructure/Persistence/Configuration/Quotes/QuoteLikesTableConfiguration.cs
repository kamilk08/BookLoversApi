using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Quotes
{
    internal class QuoteLikesTableConfiguration : EntityTypeConfiguration<QuoteLikeReadModel>
    {
        public QuoteLikesTableConfiguration()
        {
            this.ToTable("QuoteLikes");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ReaderGuid).IsRequired();

            this.Property(p => p.ReaderId).IsRequired();
        }
    }
}