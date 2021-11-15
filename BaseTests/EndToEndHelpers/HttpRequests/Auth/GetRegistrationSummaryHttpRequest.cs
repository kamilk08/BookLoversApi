using System;
using System.Net.Http;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Auth
{
    public class GetRegistrationSummaryHttpRequest : ISimpleHttpRequest
    {
        private string Email { get; }

        public GetRegistrationSummaryHttpRequest(string email)
        {
            this.Email = email;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return new HttpRequestMessage(
                HttpMethod.Get,
                new Uri("http://localhost:64892/api/auth/registration/" + this.Email));
        }
    }
}