using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Ratings.Application.Commands;
using BookLovers.Ratings.Application.WriteModels;
using BookLovers.Ratings.Infrastructure.Dtos;
using BookLovers.Ratings.Infrastructure.Queries.Authors;
using BookLovers.Ratings.Infrastructure.Queries.Books;
using BookLovers.Ratings.Infrastructure.Queries.Publishers;
using BookLovers.Ratings.Infrastructure.Queries.Ratings;
using BookLovers.Ratings.Infrastructure.Queries.Readers;
using BookLovers.Ratings.Infrastructure.Queries.Series;
using BookLovers.Ratings.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class RatingsController : ApiController
    {
        private readonly IModule<RatingsModule> _module;

        public RatingsController(IModule<RatingsModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/ratings")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddRating(AddRatingWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddRatingCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/ratings")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> ChangeRating(ChangeRatingWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ChangeRatingCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/ratings")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> RemoveRating(RemoveRatingWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveRatingCommand(writeModel));

            return validationResult.HasErrors ? this.BadRequest(validationResult.Errors) : Ok();
        }

        [HttpGet]
        [Route("api/ratings/book/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBookById([FromUri] BookByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookByIdQuery, BookDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/books")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> MultipleBooks([FromUri] MultipleBooksRatingsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<MultipleBooksRatingsQuery, List<BookDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/books/{bookId}/grouped")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BookGroupedRatingsById([FromUri] BookGroupedRatingsByIdQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<BookGroupedRatingsByIdQuery, BookGroupedRatingsDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/reader/{readerId}/books/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderBookRating([FromUri] ReaderBookRatingQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderBookRatingQuery, RatingDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/reader/{readerId}/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetReaderRatingsById([FromUri] ReaderRatingsByIdQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReaderRatingsByIdQuery, PaginatedResult<RatingDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/reader/{readerId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderRatings([FromUri] ReaderRatingsQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderRatingsQuery, ReaderRatingsDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/author/{authorId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AuthorStatistics([FromUri] AuthorRatingsQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<AuthorRatingsQuery, RatingsDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/series/{seriesId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> SeriesStatistics([FromUri] SeriesRatingsQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<SeriesRatingsQuery, RatingsDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/series")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> MultipleSeriesStatistics([FromUri] MultipleSeriesStatisticsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<MultipleSeriesStatisticsQuery, List<RatingsDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/ratings/publishers/{publisherId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> PublisherStatistics([FromUri] PublisherRatingsQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<PublisherRatingsQuery, RatingsDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}