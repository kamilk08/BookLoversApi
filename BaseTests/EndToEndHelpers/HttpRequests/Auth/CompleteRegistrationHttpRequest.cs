using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class CompleteRegistrationHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public CompleteRegistrationHttpRequest(string email, string token)
        {
            _builder = new HttpRequestBuilder();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Put)
                .WithUri("http://localhost:64892/api/auth/registration/" + email + "/" + token);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}