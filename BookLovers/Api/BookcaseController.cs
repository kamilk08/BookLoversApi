using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Application.Commands.Shelves;
using BookLovers.Bookcases.Application.WriteModels;
using BookLovers.Bookcases.Infrastructure.Dtos;
using BookLovers.Bookcases.Infrastructure.Queries.Bookcases;
using BookLovers.Bookcases.Infrastructure.Queries.Shelves;
using BookLovers.Bookcases.Infrastructure.Root;
using BookLovers.Filters;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class BookcaseController : ApiController
    {
        private readonly IModule<BookcaseModule> _module;

        public BookcaseController(IModule<BookcaseModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/bookcase/book")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> AddBookToBookcase(AddBookToBookcaseWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddToBookcaseCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/bookcase/book")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> RemoveFromBookcase(RemoveFromBookcaseWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveFromBookcaseCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/bookcase/shelves/book")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> ChangeBookShelf(
            ChangeShelfWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ChangeShelfCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/bookcase/shelves/book")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> RemoveBookFromShelf(RemoveFromShelfWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveFromShelfCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/bookcase/shelf")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> AddShelf(AddShelfWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddShelfCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("ShelfById", new { shelfId = writeModel.ShelfId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpDelete]
        [Route("api/bookcase/shelf")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> RemoveShelf(RemoveShelfWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveShelfCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/bookcase/shelf")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> ChangeShelfName(ChangeShelfNameWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ChangeShelfNameCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/bookcase")]
        [Authorize]
        [HasPermission("ToManageBookcase")]
        public async Task<IHttpActionResult> ChangeBookcaseOptions(ChangeBookcaseOptionsWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ChangeBookcaseOptionsCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/bookcases/{bookcaseId}")]
        [HasPermission("ToSeeBookcase")]
        public async Task<IHttpActionResult> GetBookcaseById([FromUri] BookcaseByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookcaseByIdQuery, BookcaseDto>(query);

            return Ok(queryResult.Value);
        }

        [HttpGet]
        [Route("api/bookcases/reader/{readerId}")]
        [HasPermission("ToSeeReaderBookcase")]
        public async Task<IHttpActionResult> GetBookcaseByReaderId([FromUri] BookcaseByReaderIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookcaseByReaderIdQuery, BookcaseDto>(query);

            return Ok(queryResult.Value);
        }

        [HttpGet]
        [Route("api/bookcases/{bookcaseId}/books/{page}/{count}/{descending}/{sortType}/{title?}")]
        [HasPermission("ToSeeBookcase")]
        public async Task<IHttpActionResult> BookcaseCollection(
            [FromUri] PaginatedBookcaseCollectionQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<PaginatedBookcaseCollectionQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/shelves/{shelfId}", Name = "ShelfById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ShelfById([FromUri] ShelfByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ShelfByIdQuery, ShelfDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/shelves/books/{bookId}/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BookOnShelves([FromUri] BookOnShelvesQuery query)
        {
            var queryResult =
                await _module
                    .ExecuteQueryAsync<BookOnShelvesQuery, PaginatedResult<KeyValuePair<string, int>>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/shelves/{shelfId}/books/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> BookOnShelfRecord([FromUri] ShelfRecordQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ShelfRecordQuery, ShelfRecordDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/{bookcaseId}/shelves/books/list")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> MultipleBookOnShelfRecords([FromUri] MultipleShelfRecordsQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<MultipleShelfRecordsQuery, List<ShelfRecordDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/books/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBookcasesWithBook([FromUri] BookcasesWithBookQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<BookcasesWithBookQuery, List<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/books/list")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetBookcasesWithBooks([FromUri] BookcasesWithMultipleBooksQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<BookcasesWithMultipleBooksQuery, List<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/bookcases/reader/{readerGuid}/book/{bookGuid}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> IsBookInReaderBookcase([FromUri] IsBookInBookcaseQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<IsBookInBookcaseQuery, bool>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}