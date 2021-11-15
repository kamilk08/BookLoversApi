using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BookLovers.Auth.Infrastructure.Services.Tokens
{
    public static class SigningCredentialsFactory
    {
        public static SigningCredentials Create(string secretKey, string algorithm = null)
        {
            return new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                algorithm ?? "HS256");
        }
    }
}