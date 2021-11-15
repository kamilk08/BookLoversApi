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
    public class ConflictResult : IHttpActionResult
    {
        public HttpRequestMessage Request { get; }

        public string Content { get; }

        public ConflictResult(HttpRequestMessage request, string content)
        {
            Request = request;
            Content = content;
        }

        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            var validationSummary = ValidationSummary.InvalidBusinessRule(Content);

            return new HttpResponseMessage(HttpStatusCode.Conflict)
            {
                RequestMessage = Request,
                Content = new StringContent(
                    JsonConvert.SerializeObject(validationSummary, Formatting.Indented),
                    Encoding.UTF8, "application/json")
            };
        }
    }
}