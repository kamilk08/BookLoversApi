using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    internal class AudiencesTableConfiguration : EntityTypeConfiguration<AudienceReadModel>
    {
        public AudiencesTableConfiguration()
        {
            ToTable("Audiences");

            HasKey(p => p.Id);

            Property(p => p.AudienceGuid).IsRequired();

            Property(p => p.RefreshTokenLifeTime).IsRequired();

            Property(p => p.AudienceType).IsRequired();

            Property(p => p.AudienceName).IsRequired();

            Property(p => p.Hash).IsRequired();

            Property(p => p.Salt).IsRequired();

            Property(p => p.AudienceState).IsRequired();
        }
    }
}