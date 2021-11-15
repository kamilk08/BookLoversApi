using System;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Filters;
using BookLovers.Readers.Application.Commands.Reviews;
using BookLovers.Readers.Application.WriteModels.Reviews;
using BookLovers.Readers.Infrastructure.Dtos.Reviews;
using BookLovers.Readers.Infrastructure.Queries.Readers.Reviews;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class ReviewsController : ApiController
    {
        private readonly IModule<ReadersModule> _module;

        public ReviewsController(IModule<ReadersModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/reviews")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddReview(ReviewWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddReviewCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("ReviewById", new { reviewId = writeModel.ReviewId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpPut]
        [Route("api/reviews")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToEditReview")]
        public async Task<IHttpActionResult> EditReview(ReviewWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new EditReviewCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/reviews/{guid}")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToRemoveReview")]
        public async Task<IHttpActionResult> RemoveReview(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveReviewCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/reviews/{guid}/like")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> LikeReview(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new LikeReviewCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/reviews/{guid}/like")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> UnLikeReview(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new UnlikeReviewCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/reviews/{guid}/spoiler")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddSpoilerTag(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new AddSpoilerTagCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/reviews/{guid}/spoiler")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> RemoveSpoilerTag(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveSpoilerTagCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/reviews/report")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> ReportReview(
            ReportReviewWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ReportReviewCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/reviews/{reviewId}", Name = "ReviewById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReviewById([FromUri] ReviewByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReviewByIdQuery, ReviewDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/reviews/reader/{readerId}/book/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderBookReview(
            [FromUri] ReaderBookReviewQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderBookReviewQuery, ReviewDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/reviews/reader/{readerId}/list/{page}/{count}/{sortType}/{descending}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderReviewsList(
            [FromUri] ReaderReviewsListQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReaderReviewsListQuery, PaginatedResult<ReviewDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/reviews/reader/{readerId}/ids/{page}/{count}/{sortType}/{descending}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderReviewsIds(
            [FromUri] ReaderReviewsIdsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReaderReviewsIdsQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/reviews/book/{bookId}/{page}/{count}/{sortType}/{descending}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReviewsPage(
            [FromUri] PaginatedBookReviewsQuery query)
        {
            var queryResult =
                await _module
                    .ExecuteQueryAsync<PaginatedBookReviewsQuery, PaginatedResult<ReviewDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}