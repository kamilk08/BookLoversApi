using System;
using System.Security.Cryptography;
using System.Text;
using BookLovers.Base.Infrastructure.Extensions;

namespace BookLovers.Auth.Infrastructure.Services.Hashing
{
    public class Pbkdf2Hasher : IHasher
    {
        private readonly int _deriveBytesIterationCount;
        private readonly int _saltSize;

        public Pbkdf2Hasher()
        {
            _deriveBytesIterationCount = 10000;
            _saltSize = 32;
        }

        public Pbkdf2Hasher(int saltSize, int iterationsCount)
        {
            if (saltSize < _saltSize)
                throw new ArgumentException("Salt size is too small.");

            _deriveBytesIterationCount = iterationsCount >= _deriveBytesIterationCount
                ? iterationsCount
                : throw new ArgumentException("Iteration count is too small");

            _saltSize = saltSize;
        }

        public string GetSalt(string input)
        {
            if (input.IsEmpty())
                throw new ArgumentException("Cannot generate salt from empty password");

            var numArray = new byte[_saltSize];

            RandomNumberGenerator.Create().GetBytes(numArray);

            return Convert.ToBase64String(numArray);
        }

        public string GetHash(string input, string salt)
        {
            if (salt.IsEmpty() || salt.IsEmpty())
                throw new ArgumentException("Input or salt cannot be empty when generating hash.");

            using (var rfc2898DeriveBytes =
                new Rfc2898DeriveBytes(input, Encoding.UTF8.GetBytes(salt), _deriveBytesIterationCount))

                return Convert.ToBase64String(rfc2898DeriveBytes.GetBytes(_saltSize));
        }
    }
}