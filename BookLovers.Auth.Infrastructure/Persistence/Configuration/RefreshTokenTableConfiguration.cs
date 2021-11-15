using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    internal class RefreshTokenTableConfiguration : EntityTypeConfiguration<RefreshTokenReadModel>
    {
        public RefreshTokenTableConfiguration()
        {
            ToTable("RefreshTokens");

            HasKey(p => p.Id);

            Property(p => p.Id).HasColumnOrder(1);

            Property(p => p.UserGuid).IsRequired();

            Property(p => p.AudienceGuid).IsRequired();

            Property(p => p.TokenGuid).IsRequired();

            Property(p => p.IssuedAt).IsRequired();

            Property(p => p.Expires).IsRequired();

            Property(p => p.RevokedAt).IsOptional();

            Property(p => p.Hash).IsRequired();

            Property(p => p.Salt).IsRequired();

            Property(p => p.ProtectedTicket).IsRequired();
        }
    }
}