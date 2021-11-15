using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Timelines;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Application.CommandHandlers.Timelines
{
    internal class AddReviewActivityHandler : ICommandHandler<AddReviewActivityInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddReviewActivityHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(AddReviewActivityInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var activity = Activity.InitiallyPublic(
                command.BookGuid,
                new ActivityContent(
                    "Reader added review",
                    command.AddedAt, ActivityType.NewReview));

            reader.AddToTimeLine(activity);

            await _unitOfWork.CommitAsync(reader, false);
        }
    }
}