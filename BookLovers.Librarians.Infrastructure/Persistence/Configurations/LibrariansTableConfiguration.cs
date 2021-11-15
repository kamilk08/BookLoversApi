using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class LibrariansTableConfiguration : EntityTypeConfiguration<LibrarianReadModel>
    {
        public LibrariansTableConfiguration()
        {
            this.ToTable("Librarians");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.ReaderGuid)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_ReaderGuid")
                {
                    IsUnique = true
                }));

            this.Property(p => p.Status).IsRequired();

            this.HasMany(p => p.Tickets);
        }
    }
}