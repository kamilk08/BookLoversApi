namespace BookLovers.Auth.Domain.Users.Services
{
    public interface IUserNameUniquenessChecker
    {
        bool IsUserNameUnique(string userName);
    }
}