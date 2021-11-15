using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class LoginUserRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestFormUrlCollection _collection;

        public LoginUserRequest()
        {
            var builder = new HttpRequestBuilder();
            builder = builder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/auth/token");

            _collection = builder.WithFormUrlEncodedContent();
        }

        public LoginUserRequest WithUserName(string email)
        {
            _collection.AddFormKeyValue("username", email);

            return this;
        }

        public LoginUserRequest WithPassword(string password)
        {
            _collection.AddFormKeyValue(nameof(password), password);
            _collection.AddFormKeyValue("grant_type", nameof(password));

            return this;
        }

        public LoginUserRequest WithClientId(string clientId)
        {
            _collection.AddFormKeyValue("client_id", clientId);

            return this;
        }

        public LoginUserRequest WithClientSecret(string secretKey)
        {
            _collection.AddFormKeyValue("client_secret", secretKey);

            return this;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _collection.PassFormValuesAsHttpContent().GetRequest();
        }
    }
}