using System.Data.Entity.ModelConfiguration;
using BookLovers.Auth.Infrastructure.Persistence.ReadModels;

namespace BookLovers.Auth.Infrastructure.Persistence.Configuration
{
    public class ResetPasswordsTableConfiguration :
        EntityTypeConfiguration<PasswordResetTokenReadModel>
    {
        public ResetPasswordsTableConfiguration()
        {
            ToTable("PasswordResetTokens");
        }
    }
}