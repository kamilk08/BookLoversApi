namespace BookLovers.Auth.Infrastructure.Services.Hashing
{
    public interface IHasher
    {
        string GetSalt(string input);

        string GetHash(string input, string salt);
    }
}