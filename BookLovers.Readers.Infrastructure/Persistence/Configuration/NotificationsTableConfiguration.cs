using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class NotificationsTableConfiguration : EntityTypeConfiguration<NotificationReadModel>
    {
        public NotificationsTableConfiguration()
        {
            this.ToTable("Notifications");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.NotificationObjects).HasColumnOrder(2);

            this.Property(p => p.AppearedAt).HasColumnOrder(3);

            this.Property(p => p.SeenAt).HasColumnOrder(4);

            this.Property(p => p.IsVisible).HasColumnOrder(5);
        }
    }
}