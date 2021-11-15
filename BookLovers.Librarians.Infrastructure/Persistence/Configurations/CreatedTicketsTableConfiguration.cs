using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class CreatedTicketsTableConfiguration : EntityTypeConfiguration<CreatedTicketReadModel>
    {
        public CreatedTicketsTableConfiguration()
        {
            this.ToTable("CreatedTickets");

            this.HasKey(p => p.Id);

            this.Property(p => p.TicketGuid).IsRequired();

            this.Property(p => p.IsSolved).IsRequired();
        }
    }
}