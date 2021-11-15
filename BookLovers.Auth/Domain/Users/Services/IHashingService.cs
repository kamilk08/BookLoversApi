using System;

namespace BookLovers.Auth.Domain.Users.Services
{
    public interface IHashingService
    {
        Tuple<string, string> CreateHashWithSalt(string input);

        string GetHash(string input, string salt);
    }
}