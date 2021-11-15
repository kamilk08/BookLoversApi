using Newtonsoft.Json;

namespace BookLovers.Services
{
    public class JsonWebToken
    {
        [JsonProperty("access_token")] public string AccessToken { get; set; }

        [JsonProperty("refresh_token")] public string RefreshToken { get; set; }

        [JsonProperty("token_type")] public string TokenType { get; set; }

        [JsonProperty("expires_in")] public long Ticks { get; set; }

        private JsonWebToken()
        {
        }

        public JsonWebToken(string accessToken, long tokenLifeTime)
        {
            AccessToken = accessToken;
            Ticks = tokenLifeTime;
        }

        public JsonWebToken(string accessToken, string refreshToken, long tokenLifeTime)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            Ticks = tokenLifeTime;
        }

        public static JsonWebToken EmptyToken()
        {
            return new JsonWebToken(string.Empty, string.Empty, -1);
        }
    }
}