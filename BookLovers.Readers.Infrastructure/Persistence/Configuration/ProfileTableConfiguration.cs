using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    internal class ProfileTableConfiguration : EntityTypeConfiguration<ProfileReadModel>
    {
        public ProfileTableConfiguration()
        {
            this.ToTable("Profiles");

            this.HasKey(k => k.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.Guid).IsRequired().HasColumnOrder(2).HasColumnName("Guid");

            this.Property(p => p.FullName).IsOptional().HasColumnOrder(3).HasMaxLength(128);

            this.Property(p => p.Country).IsOptional().HasColumnOrder(4)
                .HasMaxLength(255);

            this.Property(p => p.City).IsOptional().HasColumnName("City").HasColumnOrder(5);

            this.Property(p => p.BirthDate).IsOptional().HasColumnType("datetime2").HasPrecision(0).HasColumnOrder(6);

            this.Property(p => p.About).IsOptional().HasMaxLength(2083).HasColumnOrder(7);

            this.Property(p => p.WebSite).IsOptional().HasColumnName("WebSite")
                .HasMaxLength(2083).HasColumnOrder(8);

            this.Property(p => p.Status).IsRequired().HasColumnOrder(9);

            this.Property(p => p.ReaderId)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_ReaderId")
                    {
                        IsUnique = true
                    }));

            this.HasRequired(p => p.Reader);
        }
    }
}