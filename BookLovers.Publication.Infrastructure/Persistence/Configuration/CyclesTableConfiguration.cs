using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Publication.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Publication.Infrastructure.Persistence.Configuration
{
    internal class CyclesTableConfiguration : EntityTypeConfiguration<PublisherCycleReadModel>
    {
        public CyclesTableConfiguration()
        {
            this.ToTable("Cycles");
            this.HasKey(p => p.Id);
            this.Property(p => p.Id).HasColumnOrder(1).HasColumnName("Id");

            this.Property(p => p.Guid).IsRequired().HasColumnOrder(2)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_Guid")
                    {
                        IsUnique = true
                    }));

            this.Property(p => p.Cycle)
                .HasColumnOrder(3).HasColumnName("Cycle")
                .IsRequired().HasMaxLength(byte.MaxValue);

            this.Property(p => p.Status).IsRequired()
                .HasColumnOrder(4).HasColumnName("Status");

            this.HasOptional(p => p.Publisher);

            this.HasMany(p => p.CycleBooks)
                .WithMany().Map(cfg => cfg.MapLeftKey("CycleId")
                    .MapRightKey("BookId").ToTable("CycleBooks"));
        }
    }
}