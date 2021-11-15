using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Books;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Books
{
    internal class BooksTableConfiguration : EntityTypeConfiguration<BookReadModel>
    {
        public BooksTableConfiguration()
        {
            this.ToTable("Books");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnOrder(1);
            this.Property(p => p.Guid).IsRequired()
                .HasColumnOrder(2).HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Guid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Title).IsRequired()
                .HasColumnName("Title")
                .HasMaxLength(byte.MaxValue).HasColumnOrder(3);

            this.Property(p => p.Isbn).IsRequired()
                .HasColumnName("ISBN").HasMaxLength(30).HasColumnOrder(4);

            this.Property(p => p.Category)
                .HasColumnName("Category").HasColumnOrder(5);

            this.Property(p => p.CategoryId)
                .HasColumnName("CategoryId").IsRequired().HasColumnOrder(6);

            this.Property(p => p.SubCategory)
                .HasColumnName("SubCategory").HasColumnOrder(7);

            this.Property(p => p.SubCategoryId)
                .HasColumnName("SubCategoryId").IsRequired().HasColumnOrder(8);

            this.Property(p => p.PublicationDate)
                .IsRequired().HasColumnOrder(9)
                .HasColumnName("PublicationDate")
                .HasColumnType("datetime2").HasPrecision(0);

            this.Property(p => p.Description).IsOptional()
                .HasColumnOrder(10).HasColumnName("Description").HasMaxLength(4000);

            this.Property(p => p.DescriptionSource)
                .IsOptional().HasColumnOrder(11)
                .HasColumnName("DescriptionSource").HasMaxLength(4000);

            this.Property(p => p.CoverSource)
                .IsOptional().HasColumnOrder(12).HasColumnName("CoverSource");

            this.Property(p => p.Pages).IsOptional()
                .HasColumnName("Pages").HasColumnOrder(13);

            this.Property(p => p.Status).IsRequired()
                .HasColumnName("Status").HasColumnOrder(16);

            this.HasOptional(p => p.Publisher)
                .WithMany(p => p.Books).WillCascadeOnDelete(false);

            this.Property(p => p.PublisherId)
                .HasColumnName("PublisherId").HasColumnOrder(17);

            this.HasOptional(p => p.Series)
                .WithMany(p => p.Books).WillCascadeOnDelete(false);

            this.Property(p => p.SeriesId)
                .IsOptional().HasColumnOrder(18).HasColumnName("SeriesId");

            this.Property(p => p.PositionInSeries)
                .IsOptional().HasColumnName("SeriesPosition").HasColumnOrder(19);

            this.Property(p => p.ReaderId)
                .IsOptional().HasColumnOrder(20).HasColumnName("ReaderGuid");

            this.Property(p => p.LanguageId)
                .IsOptional().HasColumnName("LanguageId").HasColumnOrder(21);

            this.HasMany(p => p.Authors).WithMany(p => p.AuthorBooks).Map(cfg =>
            {
                cfg.ToTable("AuthorBooks");
                cfg.MapLeftKey("BookId").MapRightKey("AuthorId");
            });

            this.HasMany(p => p.Reviews).WithRequired()
                .Map(cfg => cfg.MapKey("BookId"))
                .WillCascadeOnDelete(true);
        }
    }
}