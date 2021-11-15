using System.Net;
using System.Net.Http;
using System.Text;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Responses
{
    internal class UnauthorizedResponse : HttpResponseMessage
    {
        public UnauthorizedResponse(HttpRequestMessage requestMessage, string reasonPhrase = null)
        {
            var validationSummary = ValidationSummary.Unauthorized();

            RequestMessage = requestMessage;
            Content = new StringContent(
                JsonConvert.SerializeObject(validationSummary, Formatting.Indented),
                Encoding.UTF8, "application/json");
            ReasonPhrase = reasonPhrase;
            StatusCode = HttpStatusCode.Unauthorized;
        }
    }
}