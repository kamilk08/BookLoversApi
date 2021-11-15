using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Domain.Books.Languages;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Seed.Models;
using BookLovers.Shared.Categories;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public class BooksSeedExecutor : BaseSeedExecutor, ICollectionSeedExecutor<SeedBook>, ISeedExecutor
    {
        private const int UserId = 1;

        private JsonWebToken _token;
        private List<Task<CreateBookWriteModel>> _tasks;
        private Guid _readerGuid;

        public SeedExecutorType ExecutorType => SeedExecutorType.BooksSeedExecutor;

        public BooksSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
            _tasks = new List<Task<CreateBookWriteModel>>();
        }

        public async Task SeedAsync(IEnumerable<SeedBook> seed)
        {
            var userDto = await this.GetUserByIdAsync(UserId);
            var jsonWebToken = await this.LoginAsync(userDto.Email);
            this._token = jsonWebToken;
            this._readerGuid = userDto.Guid;

            this._tasks = seed.OrderBy(p => p.Title).Select(CreateCommandAsync)
                .ToList();

            var firstAuthor = (await this._tasks.First()).BookWriteModel.Authors.FirstOrDefault();

            var booksWithFirstAuthor = this._tasks.Take(15).Skip(1).ToList();
            var restOfBooks = this._tasks.Skip(15).ToList();

            foreach (var task in booksWithFirstAuthor)
            {
                var writeModel = await task;

                writeModel.BookWriteModel.Authors.Add(firstAuthor);

                var response = await this.CreateNewBookAsync(writeModel);
            }

            foreach (var task in restOfBooks)
            {
                var writeModel = await task;

                var response = await this.CreateNewBookAsync(writeModel);
            }
        }

        private Task<HttpResponseMessage> CreateNewBookAsync(
            CreateBookWriteModel writeModel)
        {
            var request = this.RequestBuilder
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(this.CreateBookUrl())
                .WithStringContent(writeModel)
                .AddBearerToken(this._token.AccessToken)
                .GetRequest();

            return this.HttpClient.SendAsync(request);
        }

        private async Task<CreateBookWriteModel> CreateCommandAsync(
            SeedBook seedBook)
        {
            var seriesDto = await GetSeriesByNameAsync(seedBook);
            var publisherDto = await GetPublisherByNameAsync(seedBook);
            var authorDto = await GetAuthorByNameAsync(seedBook);

            var createBookWriteModel = new CreateBookWriteModel
            {
                BookWriteModel = new BookWriteModel
                {
                    BookGuid = seedBook.BookGuid,
                    Basics = CreateBasics(seedBook, publisherDto),
                    Description = CreateDescription(seedBook),
                    AddedBy = _readerGuid,
                    SeriesGuid = seriesDto?.Guid,
                    Authors = AssignBookAuthors(authorDto),
                    Cover = AddCoverInformation(),
                    Cycles = new List<Guid>(),
                    Details = CreateDetails(seedBook),
                    HashTags = new List<string>(),
                    PositionInSeries = seedBook.PositionInSeries == 0 ? default(int?) : seedBook.PositionInSeries
                },
                PictureWriteModel = new BookPictureWriteModel
                {
                    Cover = string.Empty,
                    FileName = string.Empty
                }
            };

            return createBookWriteModel;
        }

        private async Task<AuthorDto> GetAuthorByNameAsync(SeedBook seedBook)
        {
            AuthorDto authorDto = null;

            if (seedBook.Authors.Any())
            {
                var requestUri = this.AuthorByNameUrl(seedBook.Authors.First().Split('/').Last());

                var response = await this.HttpClient.GetAsync(requestUri);

                authorDto = await response.Content.ReadAsAsync<AuthorDto>();
            }

            return authorDto;
        }

        private async Task<PublisherDto> GetPublisherByNameAsync(SeedBook seedBook)
        {
            var uri = this.PublisherByNameUrl(seedBook.PublisherName);

            var response = await this.HttpClient.GetAsync(uri);

            PublisherDto publisherDto = null;

            if (response.IsSuccessStatusCode)
                publisherDto = await response.Content.ReadAsAsync<PublisherDto>();

            return publisherDto;
        }

        private async Task<SeriesDto> GetSeriesByNameAsync(SeedBook seedBook)
        {
            var response = await this.HttpClient.GetAsync(this.SeriesByNameUrl(seedBook.SeriesName));

            return await response.Content.ReadAsAsync<SeriesDto>();
        }

        private BookDetailsWriteModel CreateDetails(SeedBook seedBook) => new BookDetailsWriteModel
        {
            Language = Language.English.Value,
            Pages = seedBook.Pages
        };

        private BookCoverWriteModel AddCoverInformation() => new BookCoverWriteModel
        {
            CoverSource = null,
            CoverType = CoverType.NoCover.Value,
            IsCoverAdded = false
        };

        private List<Guid> AssignBookAuthors(AuthorDto author)
        {
            if (author == null)
                return new List<Guid>();

            return new List<Guid> { author.Guid };
        }

        private BookDescriptionWriteModel CreateDescription(SeedBook seedBook)
        {
            return new BookDescriptionWriteModel
            {
                Content = seedBook.Description,
                DescriptionSource = Guid.NewGuid().ToString()
            };
        }

        private BookBasicsWriteModel CreateBasics(
            SeedBook seedBook,
            PublisherDto publisher)
        {
            return new BookBasicsWriteModel
            {
                Isbn = seedBook.Isbn.Replace("-", string.Empty).Replace(" ", string.Empty),
                Category = Category.Fiction.Value,
                PublicationDate = seedBook.Published.Value,
                PublisherGuid = publisher != null ? publisher.Guid : Guid.Empty,
                SubCategory = SubCategory.FictionSubCategory.Fantasy.Value,
                Title = seedBook.Title
            };
        }

        private string CreateBookUrl()
        {
            return AppManager.GetConfigValue("books_url");
        }

        private string SeriesByNameUrl(string seriesName)
        {
            return AppManager.GetConfigValue("series_by_name") + $"/{seriesName}";
        }

        private string PublisherByNameUrl(string publisherName)
        {
            return AppManager.GetConfigValue("publisher_by_name") + $"/{publisherName}";
        }

        private string AuthorByNameUrl(string authorName)
        {
            return AppManager.GetConfigValue("author_by_name") + $"/{authorName}";
        }
    }
}