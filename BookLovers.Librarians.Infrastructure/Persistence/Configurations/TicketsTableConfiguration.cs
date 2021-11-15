using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class TicketsTableConfiguration : EntityTypeConfiguration<TicketReadModel>
    {
        public TicketsTableConfiguration()
        {
            this.ToTable("Tickets");
            this.HasKey(p => p.Id);
            this.Property(p => p.Guid)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(
                    new IndexAttribute("IX_Guid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Content).IsRequired();

            this.Property(p => p.Date).IsRequired();

            this.Property(p => p.Decision).IsRequired();

            this.Property(p => p.DecisionValue).IsRequired();

            this.Property(p => p.Description).IsRequired();

            this.Property(p => p.Title).IsRequired();

            this.Property(p => p.LibrarianGuid).IsOptional();

            this.Property(p => p.TicketConcern).IsRequired();

            this.Property(p => p.TicketState).IsRequired();

            this.Property(p => p.TicketStateValue).IsRequired();

            this.Property(p => p.TicketConcernValue).IsRequired();

            this.Property(p => p.TicketOwnerGuid).IsRequired();

            this.Property(p => p.TicketOwnerId).IsRequired();

            this.Property(p => p.Status).IsRequired();
        }
    }
}