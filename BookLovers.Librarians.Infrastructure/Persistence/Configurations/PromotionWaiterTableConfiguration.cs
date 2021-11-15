using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    internal class PromotionWaiterTableConfiguration :
        EntityTypeConfiguration<PromotionWaiterReadModel>
    {
        public PromotionWaiterTableConfiguration()
        {
            this.ToTable("PromotionWaiters");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.ReaderGuid).HasColumnOrder(2);

            this.Property(p => p.ReaderId).HasColumnOrder(3);

            this.Property(p => p.PromotionAvailability).HasColumnOrder(4);

            this.Property(p => p.Status).HasColumnOrder(5);
        }
    }
}