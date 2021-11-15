namespace BookLovers.Auth.Domain.Users.Services
{
    public interface IEmailUniquenessChecker
    {
        bool IsEmailUnique(string email);
    }
}