using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class LoginWithRefreshTokenRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly HttpRequestFormUrlCollection _collection;

        public LoginWithRefreshTokenRequest()
        {
            _builder = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/auth/token");

            _collection = _builder.WithFormUrlEncodedContent();
        }

        public LoginWithRefreshTokenRequest WithRefreshToken(string token)
        {
            _collection.AddFormKeyValue("refresh_token", token);
            _collection.AddFormKeyValue("grant_type", "refresh_token");

            return this;
        }

        public LoginWithRefreshTokenRequest WithClientId(string clientId)
        {
            _collection.AddFormKeyValue("client_id", clientId);

            return this;
        }

        public LoginWithRefreshTokenRequest WithClientSecret(
            string secretKey)
        {
            _collection.AddFormKeyValue("client_secret", secretKey);

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.WithFormUrlEncodedContent()
                .PassFormValuesAsHttpContent().GetRequest();
        }
    }
}