using System.Data.Entity.ModelConfiguration;
using BookLovers.Base.Infrastructure.Commands;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class InternalCommandsTable : EntityTypeConfiguration<InternalCommand>
    {
        public InternalCommandsTable()
        {
            ToTable("InternalCommands");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);
        }
    }
}