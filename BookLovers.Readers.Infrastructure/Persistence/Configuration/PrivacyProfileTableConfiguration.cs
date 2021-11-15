using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class PrivacyProfileTableConfiguration :
        EntityTypeConfiguration<ProfilePrivacyManagerReadModel>
    {
        public PrivacyProfileTableConfiguration()
        {
            this.ToTable("ProfilePrivacyManagers");

            this.HasKey(p => p.Id);

            this.Property(p => p.Guid).IsRequired();

            this.Property(p => p.Status).IsRequired();

            this.Property(p => p.ProfileGuid).IsRequired();

            this.Property(p => p.ProfileId).IsRequired();

            this.HasMany(p => p.PrivacyOptions);
        }
    }
}