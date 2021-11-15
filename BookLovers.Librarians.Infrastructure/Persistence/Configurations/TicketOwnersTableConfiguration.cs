using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class TicketOwnersTableConfiguration : EntityTypeConfiguration<TicketOwnerReadModel>
    {
        public TicketOwnersTableConfiguration()
        {
            this.ToTable("TicketOwners");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.ReaderGuid).IsRequired();

            this.Property(p => p.ReaderId).IsRequired()
                .HasColumnAnnotation("Index",
                new IndexAnnotation(new IndexAttribute("IX_ReaderId")
                {
                    IsUnique = true
                }));

            this.Property(p => p.Status).IsRequired();

            this.HasMany(p => p.Tickets);
        }
    }
}