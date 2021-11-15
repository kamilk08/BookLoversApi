using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Publication.Application.WriteModels.Series;
using BookLovers.Seed.Models;
using FluentHttpRequestBuilderLibrary;

namespace BookLovers.Seed.SeedExecutors
{
    public class SeriesSeedExecutor :
        BaseSeedExecutor,
        ICollectionSeedExecutor<SeedSeries>,
        ISeedExecutor
    {
        private const int UserId = 1;
        private JsonWebToken _token;

        public SeedExecutorType ExecutorType => SeedExecutorType.SeriesSeedExecutor;

        public SeriesSeedExecutor(IAppManager appManager)
            : base(appManager)
        {
        }

        public async Task SeedAsync(IEnumerable<SeedSeries> seed)
        {
            var userByIdAsync = await this.GetUserByIdAsync(UserId);
            this._token = await this.LoginAsync(userByIdAsync.Email);

            foreach (var seedSerieses in seed.Partition(10))
            {
                foreach (var seedSeries in seedSerieses)
                {
                    var response = await this.CreateSeriesAsync(seedSeries.Guid, seedSeries.Name);
                }
            }
        }

        private Task<HttpResponseMessage> CreateSeriesAsync(
            Guid seriesGuid,
            string seriesName)
        {
            var seriesWriteModel = new SeriesWriteModel
            {
                SeriesGuid = seriesGuid,
                SeriesName = seriesName
            };

            var request = RequestBuilder.InitializeRequest()
                .WithMethod(HttpMethod.Post)
                .WithUri(AppManager.GetConfigValue("series_url"))
                .AddBearerToken(_token.AccessToken)
                .WithStringContent(seriesWriteModel)
                .GetRequest();

            return HttpClient
                .SendAsync(request);
        }
    }
}