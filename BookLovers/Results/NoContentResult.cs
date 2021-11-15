using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookLovers.Results
{
    public class NoContentResult : IHttpActionResult
    {
        public HttpRequestMessage Request { get; }

        public NoContentResult(HttpRequestMessage request)
        {
            Request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(
            CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            return new HttpResponseMessage(HttpStatusCode.NoContent)
            {
                RequestMessage = Request
            };
        }
    }
}