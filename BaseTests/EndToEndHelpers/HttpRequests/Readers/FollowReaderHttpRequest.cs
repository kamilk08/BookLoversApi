using System;
using System.Net.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Readers.Application.WriteModels.Readers;
using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;

namespace BaseTests.EndToEndHelpers.HttpRequests.Readers
{
    public class FollowReaderHttpRequest : ISimpleHttpRequest
    {
        private readonly HttpRequestBuilder _builder;

        public FollowReaderHttpRequest(Guid followedGuid, JsonWebToken token)
        {
            _builder = new HttpRequestBuilder();

            _builder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri("http://localhost:64892/api/readers/follow")
                .WithStringContent(new ReaderFollowWriteModel
                {
                    FollowedGuid = followedGuid
                })
                .AddBearerToken(token.AccessToken);
        }

        public HttpRequestMessage ToHttpRequest()
        {
            return _builder.GetRequest();
        }
    }
}