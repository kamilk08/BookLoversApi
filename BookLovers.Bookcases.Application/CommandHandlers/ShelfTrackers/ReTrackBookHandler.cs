using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Domain.ShelfRecordTracker;

namespace BookLovers.Bookcases.Application.CommandHandlers.ShelfTrackers
{
    internal class ReTrackBookHandler : ICommandHandler<ReTrackBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReTrackBookHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(ReTrackBookInternalCommand command)
        {
            var tracker = await _unitOfWork.GetAsync<ShelfRecordTracker>(command.ShelfRecordTrackerGuid);
            var trackedBook = tracker.GetTrackedBook(command.BookGuid, command.OldShelfGuid);

            var newShelfRecord = new ShelfRecord(command.NewShelfGuid, command.BookGuid, command.ChangedAt);

            tracker.ReTrackBook(trackedBook, newShelfRecord);

            await _unitOfWork.CommitAsync(tracker);
        }
    }
}