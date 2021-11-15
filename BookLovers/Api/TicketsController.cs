using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Filters;
using BookLovers.Librarians.Application.Commands.Tickets;
using BookLovers.Librarians.Application.WriteModels;
using BookLovers.Librarians.Infrastructure.Dtos;
using BookLovers.Librarians.Infrastructure.Queries.Tickets;
using BookLovers.Librarians.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class TicketsController : ApiController
    {
        private readonly IModule<LibrarianModule> _module;

        public TicketsController(IModule<LibrarianModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/tickets")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> CreateTicket(
            CreateTicketWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new NewTicketCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : CreatedAtRoute("TicketById", new { ticketId = writeModel.TicketId }, writeModel);
        }

        [HttpPut]
        [Route("api/tickets")]
        [Authorize(Roles = "Librarian")]
        [HasPermission("ToResolveTicket")]
        public async Task<IHttpActionResult> ResolveTicket(
            ResolveTicketWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ResolveTicketCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/tickets/{ticketId}", Name = "TicketById")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToSeeTicket")]
        public async Task<IHttpActionResult> TicketById([FromUri] TicketByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<TicketByIdQuery, TicketDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/tickets/title/{solved}/{page}/{count}/{phrase?}")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> SearchTickets(
            [FromUri] TicketsByTitleQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<TicketsByTitleQuery, PaginatedResult<TicketDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}