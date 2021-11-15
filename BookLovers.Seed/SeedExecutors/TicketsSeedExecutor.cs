using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Seed.Models;
using BookLovers.Shared.SharedSexes;
using FluentHttpRequestBuilderLibrary;
using Newtonsoft.Json;

namespace BookLovers.Seed.SeedExecutors
{
    public class TicketsSeedExecutor :
        BaseSeedExecutor,
        ICollectionSeedExecutor<SeedTicket>,
        ISeedExecutor
    {
        private const int UserId = 2;
        private const string TicketAuthorEmail = "user1@gmail.com";
        private const string UserPassword = "Babcia123";
        private JsonWebToken _token;

        public SeedExecutorType ExecutorType => SeedExecutorType.UserTicketsExecutor;

        public TicketsSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(IEnumerable<SeedTicket> seed)
        {
            var userDto = await this.GetUserByIdAsync(UserId);
            this._token = await this.LoginAsync(userDto.Email, UserPassword);

            foreach (var seedTicket in seed)
            {
                var ticketData = this.CreateTicketData(seedTicket, userDto.Guid);
                var ticketResponse = await this.CreateTicketAsync(ticketData, this._token.AccessToken);
            }
        }

        private Task<HttpResponseMessage> CreateTicketAsync(
            CreateTicketWriteModel writeModel,
            string token)
        {
            var request = new HttpRequestBuilder()
                .InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(AppManager.GetConfigValue("tickets_url"))
                .WithStringContent(writeModel)
                .AddHeader("Cache-Control", "no-cache")
                .AddHeader("Connection", "keep-alive")
                .AddBearerToken(token).GetRequest();

            return this.HttpClient.SendAsync(request);
        }

        private CreateTicketWriteModel CreateTicketData(
            SeedTicket seedTicket,
            Guid readerGuid)
        {
            var authorWriteModel = new CreateAuthorWriteModel
            {
                AuthorWriteModel = new AuthorWriteModel
                {
                    AuthorBooks = new List<Guid>(),
                    AuthorGenres = new List<int>(),
                    AuthorGuid = Guid.NewGuid(),
                    Basics = new AuthorBasicsWriteModel
                    {
                        FirstName = Guid.NewGuid().ToString("N"),
                        SecondName = Guid.NewGuid().ToString("N"),
                        Sex = Sex.Male.Value
                    },
                    Description = new AuthorDescriptionWriteModel
                    {
                        AboutAuthor = Guid.NewGuid().ToString("N"),
                        DescriptionSource = "www.google.com",
                        WebSite = "www.google.com"
                    },
                    Details = new AuthorDetailsWriteModel
                    {
                        BirthPlace = "New York"
                    },
                    ReaderGuid = readerGuid
                }
            };

            return new CreateTicketWriteModel
            {
                Title = seedTicket.Title,
                CreatedAt = seedTicket.CreatedAt,
                Description = seedTicket.Description,
                TicketConcern = seedTicket.TicketConcern,
                TicketData = JsonConvert.SerializeObject(authorWriteModel),
                TicketGuid = Guid.NewGuid(),
                TicketObjectGuid = authorWriteModel.AuthorWriteModel.AuthorGuid
            };
        }
    }
}