using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class ReadersFollowsTableConfiguration : EntityTypeConfiguration<FollowReadModel>
    {
        public ReadersFollowsTableConfiguration()
        {
            this.ToTable("Followings");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);
        }
    }
}