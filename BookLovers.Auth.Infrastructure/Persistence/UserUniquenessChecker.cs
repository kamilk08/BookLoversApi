using System.Data.Entity;
using System.Linq;
using BookLovers.Auth.Domain.Users.Services;

namespace BookLovers.Auth.Infrastructure.Persistence
{
    public class UserUniquenessChecker : IUserNameUniquenessChecker, IEmailUniquenessChecker
    {
        private readonly AuthContext _context;

        public UserUniquenessChecker(AuthContext context)
        {
            _context = context;
        }

        public bool IsUserNameUnique(string userName)
        {
            return !_context.Users.AsNoTracking()
                .Include(p => p.UserName)
                .Any(p => p.UserName == userName);
        }

        public bool IsEmailUnique(string email)
        {
            return !_context.Users.AsNoTracking()
                .Include(p => p.Account)
                .Include(p => p.Account.Email)
                .Any(p => p.Account.Email == email);
        }
    }
}