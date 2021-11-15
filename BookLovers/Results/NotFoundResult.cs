using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookLovers.Results
{
    public class NotFoundResult : IHttpActionResult
    {
        public HttpRequestMessage HttpRequestMessage { get; }

        public NotFoundResult(HttpRequestMessage httpRequestMessage) => HttpRequestMessage = httpRequestMessage;

        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(CreateResponse());
        }

        private HttpResponseMessage CreateResponse()
        {
            return new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                RequestMessage = HttpRequestMessage
            };
        }
    }
}