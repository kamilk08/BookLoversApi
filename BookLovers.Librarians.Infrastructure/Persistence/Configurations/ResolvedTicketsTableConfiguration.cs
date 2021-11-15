using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class ResolvedTicketsTableConfiguration : EntityTypeConfiguration<ResolvedTicketReadModel>
    {
        public ResolvedTicketsTableConfiguration()
        {
            this.ToTable("ResolvedTickets");

            this.HasKey(p => p.Id);

            this.Property(p => p.TicketGuid).IsRequired();

            this.Property(p => p.Justification).IsRequired();

            this.Property(p => p.DecisionName).IsRequired();

            this.Property(p => p.DecisionValue).IsRequired();
        }
    }
}