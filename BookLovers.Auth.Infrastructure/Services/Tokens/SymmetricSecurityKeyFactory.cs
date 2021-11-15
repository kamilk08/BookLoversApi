using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Infrastructure.Services.Tokens
{
    public static class SymmetricSecurityKeyFactory
    {
        public static SecurityKey CreateSingleKey(string key)
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public static IEnumerable<SecurityKey> CreateKeys(IEnumerable<string> keys)
        {
            return keys.Select(CreateSingleKey);
        }
    }
}