using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class EditReviewActivityHandler : ICommandHandler<EditReviewActivityInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EditReviewActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(EditReviewActivityInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);
            var activity = Activity.InitiallyPublic(
                command.ReviewGuid,
                new ActivityContent("Reader edited review", command.EditDate, ActivityType.EditReview));

            if (reader.TimeLine.HasActivity(activity))
                reader.AddToTimeLine(activity);

            await _unitOfWork.CommitAsync(reader);
        }
    }
}