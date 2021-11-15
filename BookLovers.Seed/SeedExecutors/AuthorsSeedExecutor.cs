using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Seed.Models;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public class AuthorsSeedExecutor :
        BaseSeedExecutor,
        ICollectionSeedExecutor<SeedAuthor>
    {
        private const int UserId = 1;
        private JsonWebToken _token;
        private Guid _readerGuid;

        public SeedExecutorType ExecutorType => SeedExecutorType.AuthorsSeedExecutor;

        public AuthorsSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(IEnumerable<SeedAuthor> seed)
        {
            var userDto = await this.GetUserByIdAsync(UserId);

            this._token = await this.LoginAsync(userDto.Email);
            this._readerGuid = userDto.Guid;

            var seedAuthors = seed.OrderBy(p => p.FullName).ToList();

            foreach (var seedAuthor in seedAuthors)
            {
                var authorWriteModel =
                    this.CreateAuthorWriteModel(seedAuthor);

                var httpResponseMessage =
                    await this.SendAddAuthorRequestAsync(authorWriteModel);
            }
        }

        private Task<HttpResponseMessage> SendAddAuthorRequestAsync(CreateAuthorWriteModel writeModel)
        {
            var request = RequestBuilder
                .InitializeRequest()
                .WithUri(AppManager.GetConfigValue("authors_url"))
                .WithMethod(HttpMethod.Post)
                .WithStringContent(writeModel)
                .AddBearerToken(_token.AccessToken).GetRequest();

            return HttpClient.SendAsync(request);
        }

        private CreateAuthorWriteModel CreateAuthorWriteModel(SeedAuthor seedAuthor)
        {
            return new CreateAuthorWriteModel
            {
                AuthorWriteModel = new AuthorWriteModel
                {
                    AuthorGuid = Guid.NewGuid(),
                    AuthorBooks = new List<Guid>(),
                    AuthorGenres = CreateGenres(),
                    Basics = CreateBasics(seedAuthor),
                    Description = CreateDescription(seedAuthor),
                    Details = CreateDetails(),
                    ReaderGuid = _readerGuid
                },

                PictureWriteModel = CreatePicture()
            };
        }

        private AuthorBasicsWriteModel CreateBasics(SeedAuthor seedAuthor) => new AuthorBasicsWriteModel
        {
            FirstName = string.Empty,
            SecondName = seedAuthor.FullName.Split('/').Last(),
            Sex = Sex.Male.Value
        };

        private List<int> CreateGenres() => new List<int>
        {
            SubCategory.FictionSubCategory.Action.Value,
            SubCategory.FictionSubCategory.SciFi.Value
        };

        private AuthorDetailsWriteModel CreateDetails() => new AuthorDetailsWriteModel();

        private AuthorDescriptionWriteModel CreateDescription(
            SeedAuthor seedAuthor)
        {
            return new AuthorDescriptionWriteModel
            {
                AboutAuthor = seedAuthor.AboutAuthor,
                DescriptionSource = seedAuthor.DescriptionSource,
                WebSite = seedAuthor.WebSite
            };
        }

        private AuthorPictureWriteModel CreatePicture() => new AuthorPictureWriteModel
        {
            AuthorImage = string.Empty,
            FileName = string.Empty
        };
    }
}