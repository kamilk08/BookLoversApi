using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Application.Commands.Series;
using BookLovers.Publication.Application.WriteModels.Series;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries.Series;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class SeriesController : ApiController
    {
        private readonly IModule<PublicationModule> _module;

        public SeriesController(IModule<PublicationModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/series")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> CreateSeries(
            SeriesWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new CreateSeriesCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("SeriesById", new { seriesId = writeModel.SeriesId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpDelete]
        [Route("api/series/{guid}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> ArchiveSeries(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new ArchiveSeriesCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/series/{seriesId}", Name = "SeriesById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSeriesById([FromUri] SeriesByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<SeriesByIdQuery, SeriesDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/series/list")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetMultipleSeries(
            [FromUri] MultipleSeriesQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<MultipleSeriesQuery, List<SeriesDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/series/name/{name}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSeriesByName(
            [FromUri] SeriesByNameQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<SeriesByNameQuery, SeriesDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/series/book/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBookSeries([FromUri] BookSeriesQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookSeriesQuery, SeriesDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/series/{seriesId}/books/{page}/{count}/{descending}/{sortType?}/{title?}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetSeriesBooks(
            [FromUri] PaginatedSeriesCollectionQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedSeriesCollectionQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/series/filters/{value}/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> FindSeries([FromUri] FindSeriesQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<FindSeriesQuery, PaginatedResult<SeriesDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/series/author/{authorId}/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AuthorSeries(
            [FromUri] PaginatedAuthorSeriesQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedAuthorSeriesQuery, PaginatedResult<SeriesDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}