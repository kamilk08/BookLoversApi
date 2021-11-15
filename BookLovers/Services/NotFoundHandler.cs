using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using BookLovers.Responses;

namespace BookLovers.Services
{
    public class NotFoundHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var httpResponseMessage = await base.SendAsync(request, cancellationToken);

            return httpResponseMessage.StatusCode != HttpStatusCode.NotFound
                ? httpResponseMessage
                : new NotFoundResponse(request);
        }
    }
}