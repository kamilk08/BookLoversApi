namespace BookLovers.Auth.Infrastructure.Services
{
    public class JwtSettings
    {
        public static readonly string Issuer = nameof(Issuer);

        public static readonly string JsonWebTokenKey = "JwtKey";

        public static readonly string Audience = nameof(Audience);
    }
}