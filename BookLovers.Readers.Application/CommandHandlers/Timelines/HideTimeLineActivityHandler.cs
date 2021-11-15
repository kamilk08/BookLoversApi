using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Base.Infrastructure.Services;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class HideTimeLineActivityHandler : ICommandHandler<HideActivityCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _contextAccessor;

        public HideTimeLineActivityHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleAsync(HideActivityCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(_contextAccessor.UserGuid);

            var activity = reader.TimeLine.GetActivity(
                command.WriteModel.TimeLineObjectGuid,
                command.WriteModel.OccuredAt,
                command.WriteModel.ActivityTypeId);

            reader.HideActivity(activity);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}