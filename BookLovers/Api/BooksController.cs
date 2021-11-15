using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Filters;
using BookLovers.Publication.Application.Commands.Books;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries;
using BookLovers.Publication.Infrastructure.Queries.Books;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Results;
using Newtonsoft.Json;

namespace BookLovers.Api
{
    public class BooksController : ApiController
    {
        private readonly IModule<PublicationModule> _module;

        public BooksController(IModule<PublicationModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/books")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> CreateBook(
            CreateBookWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddBookCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("BookById", new { bookId = writeModel.BookId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpPut]
        [Route("api/books")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToEditBook")]
        public async Task<IHttpActionResult> EditBook(EditBookWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new EditBookCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/books/{guid:Guid}")]
        [Authorize]
        [HasPermission("ToArchiveBook")]
        public async Task<IHttpActionResult> ArchiveBook(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new ArchiveBookCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/books/{bookId:int}", Name = "BookById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetById([FromUri] BookByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookByIdQuery, BookDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/{bookGuid:Guid}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBookByGuid([FromUri] BookByGuidQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookByGuidQuery, BookDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/isbn/{isbn}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetByIsbn([FromUri] BookByIsbnQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookByIsbnQuery, BookDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/title/{title}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetByTitle([FromUri] BookByTitleQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookByTitleQuery, BookDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/list/ids")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBooksListByIds([FromUri] MultipleBooksByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<MultipleBooksByIdQuery, List<BookDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/list/guides")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBooksListByGuides([FromUri] MultipleBooksByGuidQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<MultipleBooksByGuidQuery, List<BookDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BooksPage([FromUri] PaginatedBooksQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedBooksQuery, PaginatedResult<BookDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/filters/{value}/{page?}/{count?}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> Search([FromUri] FindBookByTitleQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<FindBookByTitleQuery, PaginatedResult<BookDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/books/browse")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BrowseBooks([FromUri] string searchCriteria)
        {
            var criteria = JsonConvert.DeserializeObject<BookFilterCriteria>(searchCriteria);

            var queryResult =
                await _module.ExecuteQueryAsync<BookSearchQuery, PaginatedResult<BookDto>>(
                    new BookSearchQuery(criteria));

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}