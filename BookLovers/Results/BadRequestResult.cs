using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure.Validation;
using Newtonsoft.Json;

namespace BookLovers.Results
{
    public class BadRequestResult : IHttpActionResult
    {
        public HttpRequestMessage Request { get; }

        public IEnumerable<ValidationError> Errors { get; }

        public BadRequestResult(HttpRequestMessage request, IEnumerable<ValidationError> errors)
        {
            Request = request;
            Errors = errors;
        }

        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            var validationSummary = ValidationSummary.InvalidCommand(Errors);

            httpResponseMessage.Content =
                new StringContent(JsonConvert.SerializeObject(validationSummary, Formatting.Indented), Encoding.UTF8,
                    "application/json");
            httpResponseMessage.Version = Request.Version;

            return httpResponseMessage;
        }
    }
}