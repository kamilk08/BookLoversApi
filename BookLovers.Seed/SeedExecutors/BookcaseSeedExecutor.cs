using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    internal class BookcaseSeedExecutor :
        BaseSeedExecutor,
        ISimpleSeedExecutor<Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>>>,
        ISeedExecutor
    {
        private const int UserId = 1;
        private const int ReadCategoryId = 1;
        private const int CurrentlyReadingCategoryId = 2;
        private const int WantToReadCategoryId = 3;

        private JsonWebToken _token;

        public SeedExecutorType ExecutorType => SeedExecutorType.BookcaseSeedExecutor;

        public BookcaseSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(
            Tuple<IEnumerable<SeedUser>, IEnumerable<SeedBook>> seed)
        {
            var users = seed.Item1.Take(12).ToList();
            var books = seed.Item2.Skip(1).ToList();

            var booksOnReadShelf = books.Take(15).ToList();
            var booksOnCurrentlyReadingShelf = books.Skip(15).Take(15).ToList();
            var booksOnWantToReadShelf = books.Skip(30).Take(10).ToList();

            foreach (var seedUser in users)
            {
                var userDto = await GetUserByGuidAsync(seedUser.UserGuid);

                this._token = await LoginAsync(userDto.Email, seedUser.Password);

                var bookcaseDto = await this.GetReaderBookcaseAsync(userDto.ReaderId);

                foreach (var seedBook in booksOnReadShelf)
                {
                    var shelfDto = bookcaseDto.Shelves.SingleOrDefault(
                        p => p.ShelfCategory == ReadCategoryId);

                    await this.AddToBookcaseAsync(
                        bookcaseDto.Guid, seedBook.BookGuid,
                        shelfDto.Guid, this._token.AccessToken);
                }

                foreach (var seedBook in booksOnCurrentlyReadingShelf)
                {
                    var shelfDto = bookcaseDto.Shelves.SingleOrDefault(
                        p => p.ShelfCategory == CurrentlyReadingCategoryId);

                    await this.AddToBookcaseAsync(
                        bookcaseDto.Guid, seedBook.BookGuid,
                        shelfDto.Guid, this._token.AccessToken);
                }

                foreach (var seedBook in booksOnWantToReadShelf)
                {
                    var shelfDto =
                        bookcaseDto.Shelves.SingleOrDefault(
                            p => p.ShelfCategory == WantToReadCategoryId);

                    await this.AddToBookcaseAsync(bookcaseDto.Guid, seedBook.BookGuid, shelfDto.Guid,
                        this._token.AccessToken);
                }
            }
        }

        private async Task AddToBookcaseAsync(
            Guid bookcaseGuid,
            Guid bookGuid,
            Guid shelfGuid,
            string token)
        {
            if (await this.BookExistAsync(bookGuid))
                return;

            var bookcaseWriteModel = new AddBookToBookcaseWriteModel
            {
                BookcaseGuid = bookcaseGuid,
                BookGuid = bookGuid,
                ShelfGuid = shelfGuid
            };

            var request = this.RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(this.AppManager.GetConfigValue("add_to_bookcase"))
                .AddBearerToken(token).WithStringContent<AddBookToBookcaseWriteModel>(bookcaseWriteModel).GetRequest();

            var httpResponseMessage = await this.HttpClient.SendAsync(request);
        }

        private async Task<bool> BookExistAsync(Guid bookGuid)
        {
            var response = await HttpClient.GetAsync(this.BookByGuidUrl(bookGuid));

            return await response.Content.ReadAsAsync<BookDto>() == null;
        }

        private async Task<BookcaseDto> GetReaderBookcaseAsync(int userId)
        {
            var request = this.RequestBuilder.InitializeRequest()
                .WithUri(this.GetUserBookcaseUrl(userId))
                .WithMethod(HttpMethod.Get)
                .AddBearerToken(this._token.AccessToken)
                .GetRequest();

            var response = await this.HttpClient.SendAsync(request);

            return await response.Content.ReadAsAsync<BookcaseDto>();
        }

        private string GetUserBookcaseUrl(int userId)
        {
            return AppManager.GetConfigValue("user_bookcase_url") + $"/{userId}";
        }
    }
}