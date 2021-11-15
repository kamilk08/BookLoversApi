using System.Net;
using System.Net.Http;
using System.Text;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Responses
{
    internal class ForbiddenResponse : HttpResponseMessage
    {
        public ForbiddenResponse(HttpRequestMessage request, string reasonPhrase = null)
        {
            var validationSummary = ValidationSummary.Forbidden();

            RequestMessage = request;
            Content = new StringContent(
            JsonConvert.SerializeObject(validationSummary, Formatting.Indented),
            Encoding.UTF8, "application/json");
            StatusCode = HttpStatusCode.Forbidden;
            ReasonPhrase = reasonPhrase;
        }
    }
}