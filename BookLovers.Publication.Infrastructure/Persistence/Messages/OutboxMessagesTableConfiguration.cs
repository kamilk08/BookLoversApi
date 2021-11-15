using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Publication.Infrastructure.Persistence.Messages
{
    internal class OutboxMessagesTableConfiguration : EntityTypeConfiguration<OutboxMessage>
    {
        public OutboxMessagesTableConfiguration()
        {
            this.ToTable("OutboxMessages");

            this.HasKey(p => p.Guid);

            this.Property(p => p.Guid)
                .HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Guid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.OccuredAt).IsRequired().HasColumnOrder(2);

            this.Property(p => p.ProcessedAt).IsOptional().HasColumnOrder(3);

            this.Property(p => p.Type).IsRequired().HasColumnOrder(4);

            this.Property(p => p.Assembly).IsRequired().HasColumnOrder(5);

            this.Property(p => p.Data).IsRequired().HasColumnOrder(6);
        }
    }
}