// Decompiled with JetBrains decompiler
// Type: BaseTests.EndToEndHelpers.HttpRequests.Bookcase.GetReaderBookcaseHttpRequest
// Assembly: BaseTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: D6D40273-49ED-405F-84F1-EB85C3A44EE0
// Assembly location: C:\Users\Kamil\source\repos\BookLovers\BaseTests\bin\Release\BaseTests.dll

using FluentHttpRequestBuilderLibrary;
using SimpleE2ETesterLibrary.Interfaces;
using System.Net.Http;

namespace BaseTests.EndToEndHelpers.HttpRequests.Bookcase
{
  public class GetReaderBookcaseHttpRequest : ISimpleHttpRequest
  {
    private readonly HttpRequestBuilder _builder;
    private readonly int _readerId;
    private readonly string _token;

    public GetReaderBookcaseHttpRequest(int readerId, string token)
    {
      this._builder = new HttpRequestBuilder();
      this._readerId = readerId;
      this._token = token;
    }

    public HttpRequestMessage ToHttpRequest() => this._builder.InitializeRequest().WithMethod(HttpMethod.Get).WithUri(string.Format("http://localhost:64892/api/bookcases/reader/{0}", (object) this._readerId)).AddBearerToken(this._token).GetRequest();
  }
}
