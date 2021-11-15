using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Quotes;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Quotes
{
    internal class QuotesTableConfiguration : EntityTypeConfiguration<QuoteReadModel>
    {
        public QuotesTableConfiguration()
        {
            this.ToTable("Quotes");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).IsRequired().HasColumnOrder(2);

            this.Property(p => p.Quote).IsRequired().HasColumnOrder(3);

            this.Property(p => p.AddedAt).HasColumnOrder(4);

            this.Property(p => p.Status).IsRequired().HasColumnOrder(5);

            this.HasOptional(p => p.Author).WithMany(p => p.Quotes);

            this.HasOptional(p => p.Book).WithMany(p => p.Quotes);

            this.Property(p => p.ReaderGuid).IsOptional();

            this.Property(p => p.ReaderId).IsOptional();

            this.HasMany(p => p.QuoteLikes)
                .WithRequired()
                .Map(fk => fk.MapKey("QuoteId"))
                .WillCascadeOnDelete(true);
        }
    }
}