using System.Net;
using System.Net.Http;
using System.Text;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Responses
{
    public class NotFoundResponse : HttpResponseMessage
    {
        public NotFoundResponse(HttpRequestMessage request)
        {
            var validationSummary = ValidationSummary.NotFound();

            RequestMessage = request;
            Content = new StringContent(
            JsonConvert.SerializeObject(validationSummary, Formatting.Indented),
            Encoding.UTF8, "application/json");
            ReasonPhrase = "Not found";
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}