using System.Data.Entity.ModelConfiguration;
using BookLovers.Readers.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Readers.Infrastructure.Persistence.Configuration
{
    public class ProfilePrivacyOptionTableConfiguration :
        EntityTypeConfiguration<ProfilePrivacyOptionReadModel>
    {
        public ProfilePrivacyOptionTableConfiguration()
        {
            this.ToTable("PrivacyOptions");

            this.HasKey(p => p.Id);

            this.Property(p => p.PrivacyTypeId).IsRequired();

            this.Property(p => p.PrivacyTypeOptionId).IsRequired();
        }
    }
}