using System;
using System.Net.Http;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class GetReaderByGuidHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;
        private readonly Guid _readerGuid;

        public GetReaderByGuidHttpRequest(Guid readerGuid)
        {
            _builder = new HttpRequestBuilder();
            _readerGuid = readerGuid;
        }

        public HttpRequestMessage ToHttpRequest()
        {
            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Get)
                .WithUri($"http://localhost:64892/api/readers/{_readerGuid}");

            return _builder.GetRequest();
        }
    }
}