using System;
using BookLovers.Auth.Domain.Users.Services;
using BookLovers.Auth.Infrastructure.Services.Hashing;

namespace BookLovers.Auth.Infrastructure.Services
{
    public class HashingService : IHashingService
    {
        private readonly IHasher _passwordHasher;

        public HashingService(IHasher passwordHasher) =>
            _passwordHasher = passwordHasher;

        public Tuple<string, string> CreateHashWithSalt(string input)
        {
            var salt = _passwordHasher.GetSalt(input);

            return Tuple.Create(_passwordHasher.GetHash(input, salt), salt);
        }

        public string GetHash(string input, string salt)
        {
            return _passwordHasher.GetHash(input, salt);
        }
    }
}