using System.Threading.Tasks;
using BookLovers.Auth.Domain.PasswordResets;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Domain.Users.Services
{
    public interface IPasswordResetTokenRepository : IRepository<PasswordResetToken>
    {
        Task<PasswordResetToken> GetEmailAsync(string email);

        Task<PasswordResetToken> GetByGeneratedTokenAsync(string token);
    }
}