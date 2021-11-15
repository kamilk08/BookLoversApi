using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Bookcases.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Bookcases.Infrastructure.Persistence.Configuration
{
    internal class ReadersTableConfiguration : EntityTypeConfiguration<ReaderReadModel>
    {
        public ReadersTableConfiguration()
        {
            ToTable("Readers");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.ReaderId).HasColumnOrder(4)
                .IsRequired()
                .HasColumnAnnotation("Index", new IndexAnnotation(new IndexAttribute("IX_ReaderId")
                {
                    IsUnique = true
                }));

            Property(p => p.ReaderGuid).IsRequired().HasColumnOrder(2);

            Property(p => p.Status).IsRequired().HasColumnOrder(3);
        }
    }
}