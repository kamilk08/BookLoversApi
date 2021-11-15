using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;

namespace BookLovers.Auth.Domain.Users.Services
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<User> GetUserByNickNameAsync(string nickname);
    }
}