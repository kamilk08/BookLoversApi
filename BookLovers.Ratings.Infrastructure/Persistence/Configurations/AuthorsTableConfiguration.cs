using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class AuthorsTableConfiguration : EntityTypeConfiguration<AuthorReadModel>
    {
        public AuthorsTableConfiguration()
        {
            this.ToTable("Authors");

            this.HasKey(p => p.Id);

            this.Property(p => p.AuthorGuid).IsRequired();

            this.Property(p => p.AuthorId)
                .IsRequired()
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_AuthorId")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Guid).IsRequired();
        }
    }
}