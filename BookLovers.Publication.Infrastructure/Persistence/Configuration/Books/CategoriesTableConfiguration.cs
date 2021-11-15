using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class CategoriesTableConfiguration : EntityTypeConfiguration<CategoryReadModel>
    {
        public CategoriesTableConfiguration()
        {
            this.ToTable("Categories");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1).HasColumnName("Id");

            this.Property(p => p.CategoryName).HasColumnOrder(2)
                .HasColumnName("Category").IsRequired();

            this.HasMany(p => p.SubCategories)
                .WithRequired(p => p.Category).WillCascadeOnDelete(true);
        }
    }
}