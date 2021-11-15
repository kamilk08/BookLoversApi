using System.Data.Entity.ModelConfiguration;
using BookLovers.Ratings.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Ratings.Infrastructure.Persistence.Configurations
{
    internal class RatingsTableConfiguration : EntityTypeConfiguration<RatingReadModel>
    {
        public RatingsTableConfiguration()
        {
            this.ToTable("Ratings");

            this.HasKey(p => p.Id);

            this.Property(p => p.Stars).IsRequired();
        }
    }
}