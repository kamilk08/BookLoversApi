using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Base.Infrastructure.Messages;

namespace BookLovers.Librarians.Infrastructure.Persistence.Messages
{
    internal class InBoxMessagesTableConfiguration : EntityTypeConfiguration<InBoxMessage>
    {
        public InBoxMessagesTableConfiguration()
        {
            ToTable("InBoxMessages");

            HasKey(p => p.Guid);

            Property(p => p.Guid).HasColumnOrder(1)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None)
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_Guid")
                {
                    IsUnique = true
                }));

            Property(p => p.OccurredOn).IsRequired()
                .HasColumnOrder(2);

            Property(p => p.ProcessedAt).HasColumnOrder(3);

            Property(p => p.Type).IsRequired()
                .HasColumnOrder(4);

            Property(p => p.Data).IsRequired()
                .HasColumnOrder(5);
        }
    }
}