using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Readers.Store.Persistence.Configuration
{
    public class EventsTableConfiguration : EntityTypeConfiguration<EventEntity>
    {
        public EventsTableConfiguration()
        {
            ToTable("EventEntities");

            HasKey(p => p.Id,
                cfg => cfg.HasName("Id"));

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.AggregateGuid).IsRequired()
                .HasColumnName("AggregateGuid").HasColumnOrder(2);

            Property(p => p.Data).IsRequired()
                .HasColumnName("Event_Data").HasColumnOrder(3);

            Property(p => p.Type).IsRequired()
                .HasColumnName("Event_Type").HasColumnOrder(4);

            Property(p => p.Assembly).IsRequired()
                .HasColumnName("Event_Assembly").HasColumnOrder(5);

            Property(p => p.Version).IsRequired()
                .HasColumnName("Version").HasColumnOrder(6);
        }
    }
}