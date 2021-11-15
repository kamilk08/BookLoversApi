using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Filters;
using BookLovers.Readers.Application.Commands.Profile;
using BookLovers.Readers.Application.Commands.Readers;
using BookLovers.Readers.Application.WriteModels.Profiles;
using BookLovers.Readers.Application.WriteModels.Readers;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Queries.Readers;
using BookLovers.Readers.Infrastructure.Queries.Readers.Follows;
using BookLovers.Readers.Infrastructure.Queries.Readers.Profiles;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class ReadersController : ApiController
    {
        private readonly IModule<ReadersModule> _module;

        public ReadersController(IModule<ReadersModule> module)
        {
            _module = module;
        }

        [HttpPost]
        [Route("api/readers/follow")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> FollowReader(ReaderFollowWriteModel dto)
        {
            var validationResult = await _module.SendCommandAsync(new FollowReaderCommand(dto));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/readers/follow")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> UnFollowReader(ReaderFollowWriteModel dto)
        {
            var validationResult = await _module.SendCommandAsync(new UnFollowReaderCommand(dto));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/readers/avatar")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> ChangeAvatar(ChangeAvatarWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ChangeAvatarCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/readers/profile")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToEditProfile")]
        public async Task<IHttpActionResult> EditProfile(ProfileWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new EditProfileCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/readers/profile/favourites/author")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToAddOrRemoveFavourite")]
        public async Task<IHttpActionResult> AddFavouriteAuthor(
            AddFavouriteAuthorWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddFavouriteAuthorCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPost]
        [Route("api/readers/profile/favourites/book")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToAddOrRemoveFavourite")]
        public async Task<IHttpActionResult> AddFavouriteBook(
            AddFavouriteBookWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new AddFavouriteBookCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpDelete]
        [Route("api/readers/profile/favourites")]
        [Authorize(Roles = "Reader")]
        [HasPermission("ToAddOrRemoveFavourite")]
        public async Task<IHttpActionResult> RemoveFavourite(
            RemoveFavouriteWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new RemoveFavouriteCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpGet]
        [Route("api/readers/{readerId:int}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderById([FromUri] ReaderByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderByIdQuery, ReaderDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{guid:Guid}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderByGuid([FromUri] ReaderByGuidQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderByGuidQuery, ReaderDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{page}/{count}/{value?}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReadersPage([FromUri] ReadersPageQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReadersPageQuery, PaginatedResult<ReaderDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{readerId}/followers/ids/{page}/{count}/{value?}")]
        [HasPermission("ToSeeProfile")]
        public async Task<IHttpActionResult> ReaderFollowersIds(
            [FromUri] ReaderPaginatedFollowersQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReaderPaginatedFollowersQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{readerId}/followings/ids/{page}/{count}/{value?}")]
        [HasPermission("ToSeeProfile")]
        public async Task<IHttpActionResult> ReaderFollowingsIds(
            [FromUri] ReaderPaginatedFollowingsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReaderPaginatedFollowingsQuery, PaginatedResult<int>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{readerId}/profile")]
        [HasPermission("ToSeeProfile")]
        public async Task<IHttpActionResult> ReadersProfile(
            [FromUri] ReaderProfileQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderProfileQuery, ProfileDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{readerId}/statistics")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderStatistics(
            [FromUri] ReaderStatisticsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<ReaderStatisticsQuery, ReaderStatisticsDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{readerId}/profile/authors")]
        [HasPermission("ToSeeProfile")]
        public async Task<IHttpActionResult> FavouriteAuthors(
            [FromUri] FavouriteAuthorsQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<FavouriteAuthorsQuery, IEnumerable<FavouriteAuthorDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{readerId}/profile/books")]
        [HasPermission("ToSeeProfile")]
        public async Task<IHttpActionResult> FavouriteBooks(
            [FromUri] FavouriteBooksQuery query)
        {
            var queryResult =
                await _module.ExecuteQueryAsync<FavouriteBooksQuery, IEnumerable<FavouriteBookDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/readers/{followingId}/following")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> IsFollowing([FromUri] IsFollowingQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<IsFollowingQuery, bool>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}