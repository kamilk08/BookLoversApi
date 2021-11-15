using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class NotificationsWallTableConfiguration :
        EntityTypeConfiguration<NotificationWallReadModel>
    {
        public NotificationsWallTableConfiguration()
        {
            this.ToTable("NotificationWalls");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ReaderId).IsRequired().HasColumnOrder(3);

            this.Property(p => p.Status).IsRequired().HasColumnOrder(4);

            this.HasMany(p => p.Notifications);
        }
    }
}