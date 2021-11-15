using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class FavouritesTableConfiguration : EntityTypeConfiguration<FavouriteReadModel>
    {
        public FavouritesTableConfiguration()
        {
            this.ToTable("Favourites");

            this.Property(p => p.FavouriteGuid)
                .HasColumnAnnotation(
                    "Index",
                    new IndexAnnotation(new IndexAttribute("IX_FavouriteGuid")
                    {
                        IsUnique = true
                    }));

            this.HasMany(p => p.FavouriteOwners);
        }
    }
}