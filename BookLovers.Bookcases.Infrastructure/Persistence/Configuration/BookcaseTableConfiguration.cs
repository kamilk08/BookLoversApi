using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class BookcaseTableConfiguration : EntityTypeConfiguration<BookcaseReadModel>
    {
        public BookcaseTableConfiguration()
        {
            ToTable("Bookcases");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.Guid).IsRequired()
                .HasColumnOrder(2).HasColumnName("Guid");

            Property(p => p.Status).HasColumnOrder(3).IsRequired();

            HasMany(p => p.Shelves).WithRequired(p => p.Bookcase)
                .Map(cfg => cfg.ToTable("Shelves")
                    .MapKey("BookcaseId")).WillCascadeOnDelete(true);

            HasIndex(p => p.ReaderId).IsUnique();
        }
    }
}