using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    internal class RolesTableConfiguration : EntityTypeConfiguration<UserRoleReadModel>
    {
        public RolesTableConfiguration()
        {
            ToTable("Roles");

            HasKey(p => p.Value);
        }
    }
}