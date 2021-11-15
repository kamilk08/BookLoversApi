using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels.Authors;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration.Authors
{
    internal class AuthorsTableConfiguration : EntityTypeConfiguration<AuthorReadModel>
    {
        public AuthorsTableConfiguration()
        {
            this.ToTable("Authors");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid)
                .HasColumnName("Guid").IsRequired()
                .HasColumnOrder(2)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Guid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.FullName)
                .HasColumnName("FullName")
                .IsRequired().HasColumnOrder(3).HasMaxLength(256);

            this.Property(p => p.FirstName).HasColumnOrder(4)
                .HasMaxLength(128);

            this.Property(p => p.SecondName).HasColumnOrder(5)
                .IsRequired().HasMaxLength(128);

            this.Property(p => p.Status).HasColumnName("Status")
                .IsRequired().HasColumnOrder(6);

            this.Property(p => p.BirthDate).HasColumnName("BirthDate")
                .IsOptional().HasColumnOrder(7).HasColumnType("datetime2").HasPrecision(0);

            this.Property(p => p.DeathDate).HasColumnName("DeathDate")
                .IsOptional().HasColumnOrder(8).HasColumnType("datetime2").HasPrecision(0);

            this.Property(p => p.BirthPlace).IsOptional()
                .HasColumnName("BirthPlace").HasColumnOrder(9).HasMaxLength(byte.MaxValue);

            this.Property(p => p.AboutAuthor).IsOptional()
                .HasColumnName("AboutAuthor").HasColumnOrder(10).HasMaxLength(2083);

            this.Property(p => p.DescriptionSource)
                .IsOptional().HasColumnName("DescriptionSource")
                .HasColumnOrder(11).HasMaxLength(2083);

            this.Property(p => p.AuthorWebsite).IsOptional()
                .HasColumnName("WebSite").HasColumnOrder(12).HasMaxLength(2083);

            this.HasMany(p => p.AuthorFollowers).WithRequired(p => p.Author);

            this.HasOptional(p => p.AddedBy);

            this.HasMany(p => p.SubCategories)
                .WithMany().Map(cfg => cfg
                    .MapLeftKey("AuthorId")
                    .MapRightKey("SubCategoryId")
                    .ToTable("AuthorCategories"));

            this.HasMany(p => p.Quotes).WithOptional(p => p.Author)
                .WillCascadeOnDelete(true);
        }
    }
}