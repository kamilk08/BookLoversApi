using System;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Publication.Application.Commands.Publishers;
using BookLovers.Publication.Application.WriteModels.Publisher;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries.Publishers;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class PublishersController : ApiController
    {
        private readonly IModule<PublicationModule> _module;

        public PublishersController(IModule<PublicationModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/publishers")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> CreatePublisher(AddPublisherWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new CreatePublisherCommand(writeModel));

            return !validationResult.HasErrors
                ? CreatedAtRoute("PublisherById", new { publisherId = writeModel.PublisherId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpDelete]
        [Route("api/publishers/{guid:Guid}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> ArchivePublisher(Guid guid)
        {
            var validationResult = await _module.SendCommandAsync(new ArchivePublisherCommand(guid));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/publishers/{publisherId}", Name = "PublisherById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPublisherById([FromUri] PublisherByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<PublisherByIdQuery, PublisherDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/publishers/book/{bookId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPublisherByBookId([FromUri] PublisherByBookIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<PublisherByBookIdQuery, PublisherDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/publishers/name/{name}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPublisherByName([FromUri] PublisherByNameQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<PublisherByNameQuery, PublisherDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/publishers/{publisherId}/books/{page}/{count}/{descending}/{sortType?}/{title?}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetPublishersCollection([FromUri] PaginatedPublishersCollectionQuery query)
        {
            var queryResult = await _module
                .ExecuteQueryAsync<PaginatedPublishersCollectionQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/publishers/filters/{value}/{page?}/{count?}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> FindPublisher([FromUri] FindPublisherQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<FindPublisherQuery, PaginatedResult<PublisherDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}