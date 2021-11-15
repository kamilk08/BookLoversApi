using System;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Application.Commands.PublisherCycles;
using BookLovers.Publication.Application.WriteModels.PublisherCycles;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries.PublisherCycles;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class PublisherCyclesController : ApiController
    {
        private readonly IModule<PublicationModule> _module;

        public PublisherCyclesController(IModule<PublicationModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/cycles")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddCycle(AddCycleWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddPublisherCycleCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : CreatedAtRoute("CycleById", new { id = writeModel.PublisherCycleId }, writeModel);
        }

        [HttpPut]
        [Route("api/cycles/book")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> AddCycleBook(AddCycleBookWriteModel writeModel)
        {
            var validationResult =
                await _module.SendCommandAsync(
                    new AddPublisherCycleBookCommand(writeModel.CycleGuid, writeModel.BookGuid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/cycles/book")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> RemoveCycleBook(RemoveCycleBookWriteModel writeModel)
        {
            var validationResult =
                await _module.SendCommandAsync(
                    new RemovePublisherCycleBookCommand(writeModel.BookGuid, writeModel.CycleGuid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/cycles/{guid:Guid}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> ArchiveCycle(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new ArchivePublisherCycleCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/cycles/{id}", Name = "CycleById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCycleById([FromUri] CycleByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<CycleByIdQuery, PublisherCycleDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/cycles/name/{name}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCycleByName([FromUri] CycleByNameQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<CycleByNameQuery, PublisherCycleDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/cycles/list")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetCycles([FromUri] PaginatedCyclesQuery query)
        {
            var queryResult = await _module
                .ExecuteQueryAsync<PaginatedCyclesQuery, PaginatedResult<PublisherCycleDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/cycles/filters/{value}/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> FindCycle([FromUri] FindCycleQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<FindCycleQuery, PaginatedResult<PublisherCycleDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}