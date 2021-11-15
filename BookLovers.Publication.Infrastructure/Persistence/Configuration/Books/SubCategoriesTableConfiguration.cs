using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class SubCategoriesTableConfiguration : EntityTypeConfiguration<SubCategoryReadModel>
    {
        public SubCategoriesTableConfiguration()
        {
            this.ToTable("SubCategories");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            this.Property(p => p.Id).HasColumnOrder(1)
                .HasColumnName("Id");
            this.Property(p => p.SubCategoryName).HasColumnOrder(2)
                .HasColumnName("SubCategory").IsRequired();

            this.HasRequired(p => p.Category);
        }
    }
}