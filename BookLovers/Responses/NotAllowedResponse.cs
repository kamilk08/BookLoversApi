using System.Net;
using System.Net.Http;
using System.Text;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Responses
{
    public class NotAllowedResponse : HttpResponseMessage
    {
        public NotAllowedResponse(HttpRequestMessage request)
        {
            var validationSummary = ValidationSummary.MethodNotAllowed();

            RequestMessage = request;
            Content = new StringContent(
            JsonConvert.SerializeObject(validationSummary, Formatting.Indented),
            Encoding.UTF8, "application/json");
            ReasonPhrase = "Not allowed";
            StatusCode = HttpStatusCode.MethodNotAllowed;
        }
    }
}