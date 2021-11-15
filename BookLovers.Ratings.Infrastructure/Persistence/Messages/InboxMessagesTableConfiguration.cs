using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Ratings.Infrastructure.Persistence.Messages
{
    internal class InboxMessagesTableConfiguration : EntityTypeConfiguration<InBoxMessage>
    {
        public InboxMessagesTableConfiguration()
        {
            this.ToTable("InBoxMessages");

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

            this.Property(p => p.OccurredOn).IsRequired().HasColumnOrder(2);

            this.Property(p => p.ProcessedAt).HasColumnOrder(3);

            this.Property(p => p.Type).IsRequired().HasColumnOrder(4);

            this.Property(p => p.Data).IsRequired().HasColumnOrder(5);
        }
    }
}