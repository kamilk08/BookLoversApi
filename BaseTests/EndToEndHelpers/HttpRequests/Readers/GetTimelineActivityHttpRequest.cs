using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class GetTimelineActivityHttpRequest : ISimpleHttpRequest
    {
        private readonly int _page;
        private readonly int _count;
        private readonly bool _hidden;
        private readonly int _timelineId;
        private readonly HttpRequestBuilder _builder;

        public GetTimelineActivityHttpRequest(int timelineId, int page, int count, bool hidden)
        {
            this._builder = new HttpRequestBuilder();
            this._page = page;
            this._count = count;
            this._hidden = hidden;
            this._timelineId = timelineId;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            this._builder.InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri(
                    $"http://localhost:64892/api/timelines/{(object) this._timelineId}/activities/{(object) this._page}/{(object) this._count}/{(object) this._hidden}");

            return this._builder.GetRequest();
        }
    }
}