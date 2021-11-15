using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.WriteModels.Quotes;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public class QuotesSeedExecutor :
        BaseSeedExecutor,
        ICollectionSeedExecutor<SeedQuote>,
        ISeedExecutor
    {
        private const int UserId = 1;
        private const int BookId = 1;

        private readonly int _authorId = 48;

        private JsonWebToken _token;

        public SeedExecutorType ExecutorType => SeedExecutorType.QuotesSeedExecutor;

        public QuotesSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(IEnumerable<SeedQuote> seed)
        {
            var count = seed.Count() / 2;
            var firstHalf = seed.Take(count).ToList();
            var secondHalf = seed.Skip(count).Take(count).ToList();

            var userByIdAsync = await this.GetUserByIdAsync(UserId);
            this._token = await this.LoginAsync(userByIdAsync.Email);

            var authorResponse = await this.HttpClient.GetAsync(this.AuthorByIdUrl());

            var authorDto = await authorResponse.Content
                .ReadAsAsync<AuthorDto>();

            var bookResponse = await this.HttpClient.GetAsync(this.BookByIdUrl(BookId));

            var bookDto = await bookResponse.Content.ReadAsAsync<BookDto>();

            foreach (var seedQuote in firstHalf)
                await this.CreateQuoteAsync(seedQuote, authorDto.Guid,
                    this.CreateAuthorQuoteUrl(), this._token.AccessToken);

            foreach (var seedQuote in secondHalf)
                await this.CreateQuoteAsync(seedQuote, bookDto.Guid,
                    this.CreateBookQuoteUrl(), this._token.AccessToken);
        }

        private async Task CreateQuoteAsync(
            SeedQuote seedQuote,
            Guid quotedGuid,
            string url,
            string token)
        {
            var addQuoteWriteModel = new AddQuoteWriteModel
            {
                QuoteGuid = Guid.NewGuid(),
                Quote = seedQuote.Quote,
                AddedAt = seedQuote.AddedAt,
                QuotedGuid = quotedGuid
            };

            var request = this.RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(url)
                .AddBearerToken(token)
                .WithStringContent(addQuoteWriteModel).GetRequest();

            var httpResponseMessage = await this.HttpClient.SendAsync(request);
        }

        private string CreateAuthorQuoteUrl()
        {
            return AppManager.GetConfigValue("add_author_quote");
        }

        private string CreateBookQuoteUrl()
        {
            return AppManager.GetConfigValue("add_book_quote");
        }

        private string AuthorByIdUrl()
        {
            return AppManager.GetConfigValue("authors_url") + $"/{_authorId}";
        }
    }
}