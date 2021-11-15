using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class GetReaderTimelineHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly int _readerId;

        public GetReaderTimelineHttpRequest(int readerId)
        {
            this._builder = new HttpRequestBuilder();
            this._readerId = readerId;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return this._builder.InitializeRequest().WithMethod(HttpMethod.Get)
                .WithUri($"http://localhost:64892/api/timelines/reader/{this._readerId}")
                .GetRequest();
        }
    }
}