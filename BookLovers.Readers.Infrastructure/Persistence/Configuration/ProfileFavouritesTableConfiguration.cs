using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class ProfileFavouritesTableConfiguration :
        EntityTypeConfiguration<ProfileFavouriteReadModel>
    {
        public ProfileFavouritesTableConfiguration()
        {
            this.ToTable("ProfileFavourites");

            this.HasKey(p => p.Id);

            this.Property(p => p.Id).HasColumnOrder(1);

            this.Property(p => p.FavouriteGuid).HasColumnOrder(2);

            this.Property(p => p.FavouriteType).HasColumnOrder(3);

            this.Property(p => p.ReaderId).HasColumnOrder(4);
        }
    }
}