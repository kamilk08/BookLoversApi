using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Domain.Tokens.Services
{
    public interface ITokenRepository : IRepository<RefreshToken>
    {
        Task<RefreshToken> GetTokenAsync(string token);
    }
}