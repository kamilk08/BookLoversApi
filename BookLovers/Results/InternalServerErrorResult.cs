using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BookLovers.Results
{
    public class InternalServerErrorResult : IHttpActionResult
    {
        public HttpRequestMessage Request { get; }

        public string Content { get; }

        public InternalServerErrorResult(HttpRequestMessage request, string content)
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
            return new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                RequestMessage = Request,
                Content = new StringContent(Content, Encoding.UTF8, "application/json")
            };
        }
    }
}