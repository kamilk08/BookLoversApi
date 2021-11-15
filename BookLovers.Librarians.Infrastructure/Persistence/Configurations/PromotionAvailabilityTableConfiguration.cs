using BookLovers.Librarians.Infrastructure.Persistence.ReadModels;
using System.Data.Entity.ModelConfiguration;

namespace BookLovers.Librarians.Infrastructure.Persistence.Configurations
{
    public class PromotionAvailabilityTableConfiguration :
        EntityTypeConfiguration<PromotionAvailabilityReadModel>
    {
        public PromotionAvailabilityTableConfiguration()
        {
            this.ToTable("PromotionAvailabilities");

            this.HasKey(p => p.Id);

            this.Property(p => p.AvailabilityId).IsRequired();

            this.Property(p => p.Name).IsRequired();
        }
    }
}