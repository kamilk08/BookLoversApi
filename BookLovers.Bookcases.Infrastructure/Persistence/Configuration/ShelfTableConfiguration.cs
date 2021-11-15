using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class ShelfTableConfiguration : EntityTypeConfiguration<ShelfReadModel>
    {
        public ShelfTableConfiguration()
        {
            ToTable("Shelves");

            HasKey(k => k.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.Guid).IsRequired().HasColumnOrder(2);

            Property(p => p.ShelfName).IsRequired()
                .HasColumnName("ShelfName").HasColumnOrder(3)
                .HasMaxLength(255);

            Property(p => p.ShelfCategory)
                .IsRequired().HasColumnName("ShelfCategory")
                .HasColumnOrder(4);

            HasRequired(p => p.Bookcase);

            HasMany(p => p.Books).WithMany().Map(cfg =>
                cfg.ToTable("ShelvesWithBooks")
                    .MapLeftKey("ShelfRowId")
                    .MapRightKey("BookRowId"));
        }
    }
}