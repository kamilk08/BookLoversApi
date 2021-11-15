using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    public class SettingsManagersTableConfiguration : EntityTypeConfiguration<SettingsManagerReadModel>
    {
        public SettingsManagersTableConfiguration()
        {
            ToTable("SettingsManagers");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.Guid).HasColumnOrder(2);

            Property(p => p.BookcaseGuid).HasColumnOrder(3);

            Property(p => p.BookcaseId).HasColumnOrder(4);

            Property(p => p.Privacy).HasColumnOrder(5);

            Property(p => p.Capacity).HasColumnOrder(6);
        }
    }
}