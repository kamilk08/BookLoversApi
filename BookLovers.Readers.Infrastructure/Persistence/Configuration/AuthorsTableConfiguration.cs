using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class AuthorsTableConfiguration : EntityTypeConfiguration<AuthorReadModel>
    {
        public AuthorsTableConfiguration()
        {
            this.ToTable("Authors");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.AuthorGuid).HasColumnOrder(2);

            this.Property(p => p.AuthorId).HasColumnOrder(3)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_AuthorId")
                    {
                        IsUnique = true
                    }));
        }
    }
}