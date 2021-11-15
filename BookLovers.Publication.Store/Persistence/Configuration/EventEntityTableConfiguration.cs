using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Publication.Store.Persistence.Configuration
{
    internal class EventEntityTableConfiguration : EntityTypeConfiguration<EventEntity>
    {
        public EventEntityTableConfiguration()
        {
            this.ToTable("EventEntities");
            this.HasKey(p => p.Id, cfg => cfg.HasName("Id"));
            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.AggregateGuid)
                .IsRequired().HasColumnName("AggregateGuid")
                .HasColumnOrder(2);

            this.Property(p => p.Data).IsRequired()
                .HasColumnName("Event_Data").HasColumnOrder(3);

            this.Property(p => p.Type).IsRequired()
                .HasColumnName("Event_Type").HasColumnOrder(4)
                ;
            this.Property(p => p.Assembly).IsRequired()
                .HasColumnName("Event_Assembly").HasColumnOrder(5);

            this.Property(p => p.Version).IsRequired()
                .HasColumnName("Version").HasColumnOrder(6);
        }
    }
}