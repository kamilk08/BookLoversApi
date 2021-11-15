using System.Security.Cryptography;
using System.Text;
using BookLovers.Auth.Application.Contracts.Tokens;

namespace BookLovers.Auth.Infrastructure.Services.Tokens
{
    internal class RandomSecretKeyGenerator : IRandomSecretKeyGenerator
    {
        public string CreateSecretKey()
        {
            var numArray = new byte[32];

            RandomNumberGenerator.Create().GetBytes(numArray);

            return Encoding.UTF8.GetString(numArray);
        }
    }
}