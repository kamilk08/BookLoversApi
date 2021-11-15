using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Responses
{
    internal class BadRequestResponse : HttpResponseMessage
    {
        public BadRequestResponse(HttpRequestMessage request, string reasonPhrase)
        {
            RequestMessage = request;
            ReasonPhrase = reasonPhrase;
            StatusCode = HttpStatusCode.BadRequest;
        }

        public BadRequestResponse(HttpRequestMessage request, SimpleError error, string reasonPhrase = null)
        {
            var validationSummary = ValidationSummary.InvalidCommand(
                new List<ValidationError>() { error });

            RequestMessage = request;
            Content = new StringContent(
                JsonConvert.SerializeObject(validationSummary, Formatting.Indented),
                Encoding.UTF8, "application/json");
            ReasonPhrase = reasonPhrase;
            StatusCode = HttpStatusCode.BadRequest;
        }
    }
}