using System;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Filters;
using BookLovers.Publication.Application.Commands.Quotes;
using BookLovers.Publication.Application.WriteModels.Quotes;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries.Quotes;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class QuotesController : ApiController
    {
        private readonly IModule<PublicationModule> _module;

        public QuotesController(IModule<PublicationModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/quotes/author")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddAuthorQuote(AddQuoteWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddAuthorQuoteCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("getQuoteById", new { quoteId = writeModel.QuoteId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpPost]
        [Route("api/quotes/book")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddBookQuote(AddQuoteWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddBookQuoteCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("getQuoteById", new { quoteId = writeModel.QuoteId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpDelete]
        [Route("api/quotes/{guid}")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToArchiveQuote")]
        public async Task<IHttpActionResult> ArchiveQuote(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new ArchiveQuoteCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/quotes/{guid}/like")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> LikeQuote(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new LikeQuoteCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/quotes/{guid}/like")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> UnLikeQuote(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new UnLikeQuoteCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/quotes/{quoteId}", Name = "getQuoteById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetQuoteById([FromUri] QuoteByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<QuoteByIdQuery, QuoteDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/quotes/author/{authorId}/{page}/{count}/{order}/{descending}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAuthorQuotes([FromUri] PaginatedAuthorQuotesQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedAuthorQuotesQuery, PaginatedResult<QuoteDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/quotes/book/{bookId}/{page}/{count}/{order}/{descending}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBookQuotes([FromUri] PaginatedBookQuotesQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedBookQuotesQuery, PaginatedResult<QuoteDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/quotes/reader/{readerId}/{page}/{count}/{order}/{descending}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetReaderQuotes([FromUri] PaginatedUserQuotesQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedUserQuotesQuery, PaginatedResult<QuoteDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}