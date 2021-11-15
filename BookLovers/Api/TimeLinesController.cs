using System.Threading.Tasks;
using System.Web.Http;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Queries;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Application.WriteModels.Timelines;
using BookLovers.Readers.Infrastructure.Dtos.Readers;
using BookLovers.Readers.Infrastructure.Queries.Readers.TimeLines;
using BookLovers.Readers.Infrastructure.Root;
using BookLovers.Results;

namespace BookLovers.Api
{
    public class TimeLinesController : ApiController
    {
        private readonly IModule<ReadersModule> _module;

        public TimeLinesController(IModule<ReadersModule> module)
        {
            _module = module;
        }

        [HttpPut]
        [Route("api/timelines/activity/hide")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> HideActivity(
            HideTimeLineActivityWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new HideActivityCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/timelines/activity/show")]
        [Authorize(Roles = "Reader")]
        public async Task<IHttpActionResult> ShowActivity(
            ShowTimeLineActivityWriteModel writeModel)
        {
            var validationResult = await _module.SendCommandAsync(new ShowActivityCommand(writeModel));

            return validationResult.HasErrors
                ? this.BadRequest(validationResult.Errors)
                : Ok();
        }

        [HttpPut]
        [Route("api/timelines/{timelineId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> TimeLineById([FromUri] TimeLineByIdQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<TimeLineByIdQuery, TimeLineDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/timelines/reader/{readerId}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderTimeline(
            [FromUri] ReaderTimeLineQuery query)
        {
            var queryResult = await _module.ExecuteQueryAsync<ReaderTimeLineQuery, TimeLineDto>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }

        [HttpGet]
        [Route("api/timelines/{timelineId}/activities/{page}/{count}/{hidden}")]
        [AllowAnonymous]
        public async Task<IHttpActionResult> ReaderTimelineActivities(
            [FromUri] TimelineActivitiesQuery query)
        {
            var queryResult = await _module
                .ExecuteQueryAsync<TimelineActivitiesQuery, PaginatedResult<TimeLineActivityDto>>(query);

            return !queryResult.HasErrors
                ? Ok(queryResult.Value)
                : this.BadRequest(queryResult.QueryErrors);
        }
    }
}