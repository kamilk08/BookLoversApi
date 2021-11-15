using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Filters;
using BookLovers.Publication.Application.Commands.Authors;
using BookLovers.Publication.Application.WriteModels.Author;
using BookLovers.Publication.Application.WriteModels.Books;
using BookLovers.Publication.Infrastructure.Dtos.Publications;
using BookLovers.Publication.Infrastructure.Queries.Authors;
using BookLovers.Publication.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class AuthorsController : ApiController
    {
        private readonly IModule<PublicationModule> _module;

        public AuthorsController(IModule<PublicationModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/authors")]
        [Authorize(Roles = "Librarian")]
        public async Task<IHttpActionResult> CreateAuthor(
            CreateAuthorWriteModel writeModel)
        {
            var validationResult = await this._module.SendCommandAsync(new CreateAuthorCommand(writeModel));
            return !validationResult.HasErrors
                ? this.CreatedAtRoute("AuthorById", new { authorId = writeModel.AuthorWriteModel.AuthorId }, writeModel)
                : this.BadRequest(validationResult.Errors);
        }

        [HttpPut]
        [Route("api/authors")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToEditAuthor")]
        public async Task<IHttpActionResult> EditAuthor(
            EditAuthorWriteModel writeModel)
        {
            var validationResult = await this._module.SendCommandAsync(new EditAuthorCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : this.Ok();
        }

        [HttpPost]
        [Route("api/authors/{authorGuid}/follow")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> FollowAuthor(Guid? authorGuid)
        {
            var validationResult =
                await this._module.SendCommandAsync(new FollowAuthorCommand(authorGuid.GetValueOrDefault()));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : this.Ok();
        }

        [HttpDelete]
        [Route("api/authors/{authorGuid}/follow")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> UnFollowAuthor(Guid? authorGuid)
        {
            var validationResult =
                await this._module.SendCommandAsync(new UnFollowAuthorCommand(authorGuid.GetValueOrDefault()));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : this.Ok();
        }

        [HttpDelete]
        [Route("api/authors/{guid}")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToArchiveAuthor")]
        public async Task<IHttpActionResult> ArchiveAuthor(Guid? guid)
        {
            var validationResult =
                await this._module.SendCommandAsync(new ArchiveAuthorCommand(guid.GetValueOrDefault()));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : this.Ok();
        }

        [HttpGet]
        [Route("api/authors/{authorId}", Name = "AuthorById")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAuthorById([FromUri] AuthorByIdQuery query)
        {
            var queryResult = await this._module.ExecuteQueryAsync<AuthorByIdQuery, AuthorDto>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/authors/{authorGuid:Guid}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAuthorByGuid(
            [FromUri] AuthorByGuidQuery query)
        {
            var queryResult = await this._module.ExecuteQueryAsync<AuthorByGuidQuery, AuthorDto>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/authors/name/{name}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAuthorByName(
            [FromUri] AuthorByNameQuery query)
        {
            var queryResult = await this._module.ExecuteQueryAsync<AuthorByNameQuery, AuthorDto>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/authors/filters/{value}/{page}/{count}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> FindAuthor([FromUri] PaginatedAuthorsQuery query)
        {
            var queryResult =
                await this._module.ExecuteQueryAsync<PaginatedAuthorsQuery, PaginatedResult<AuthorDto>>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/authors/list/ids")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAuthors([FromUri] MultipleAuthorsQuery query)
        {
            var queryResult = await this._module.ExecuteQueryAsync<MultipleAuthorsQuery, IList<AuthorDto>>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/authors/list/guides")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> GetAuthorsByGuides(
            [FromUri] MultipleAuthorsByGuidQuery query)
        {
            var queryResult = await this._module.ExecuteQueryAsync<MultipleAuthorsByGuidQuery, IList<AuthorDto>>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/authors/{authorId}/books/{page}/{count}/{descending?}/{sortType?}/{title?}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> AuthorsCollection(
            [FromUri] AuthorsCollectionQuery query)
        {
            var queryResult = await this._module.ExecuteQueryAsync<AuthorsCollectionQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? this.Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}