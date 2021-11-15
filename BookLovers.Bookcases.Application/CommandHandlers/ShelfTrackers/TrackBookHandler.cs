using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Domain.ShelfRecordTracker;

namespace BookLovers.Bookcases.Application.CommandHandlers.ShelfTrackers
{
    internal class TrackBookHandler : ICommandHandler<TrackBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrackBookHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(TrackBookInternalCommand command)
        {
            var tracker = await _unitOfWork.GetAsync<ShelfRecordTracker>(command.ShelfRecordTrackerGuid);

            tracker.TrackBook(new ShelfRecord(command.ShelfGuid, command.BookGuid, command.TrackedAt));

            await _unitOfWork.CommitAsync(tracker);
        }
    }
}