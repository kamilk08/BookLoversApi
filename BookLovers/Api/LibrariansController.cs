using System;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Librarians.Application.Commands;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Queries.Librarians;
using BookLovers.Librarians.Infrastructure.Queries.PromotionWaiters;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class LibrariansController : ApiController
    {
        private readonly IModule<LibrarianModule> _module;

        public LibrariansController(IModule<LibrarianModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/librarians")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IHttpActionResult> CreateLibrarian(CreateLibrarianWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new CreateLibrarianCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute(
                    "GetLibrarianById",
                    new { librarianId = writeModel.LibrarianId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpDelete]
        [Route("api/librarians/{guid}")]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IHttpActionResult> SuspendLibrarian(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new SuspendLibrarianCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/librarians/{librarianId}", Name = "GetLibrarianById")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> LibrarianById([FromUri] LibrarianByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<LibrarianByIdQuery, LibrarianDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/librarians/reader/{guid}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> LibrarianByReaderGuid([FromUri] LibrarianByReaderGuidQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<LibrarianByReaderGuidQuery, LibrarianDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/librarians/list")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> GetLibrariansPage([FromUri] LibrarianPageQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<LibrarianPageQuery, PaginatedResult<LibrarianDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/librarians/promotions")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> GetPromotionWaiters([FromUri] PaginatedPromotionWaitersQuery query)
        {
            var queryResult =
                await _module
                    .ExecuteQueryAsync<PaginatedPromotionWaitersQuery, PaginatedResult<PromotionWaiterDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/librarians/tickets/{solved}/{page}/{count}/{phrase?}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> ManageableTickets([FromUri] ManageableTicketsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ManageableTicketsQuery, PaginatedResult<TicketDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}