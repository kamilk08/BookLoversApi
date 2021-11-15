using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Domain.ShelfRecordTracker;

namespace BookLovers.Bookcases.Application.CommandHandlers.ShelfTrackers
{
    internal class UnTrackBookHandler : ICommandHandler<UnTrackBookInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnTrackBookHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(UnTrackBookInternalCommand command)
        {
            var tracker = await _unitOfWork.GetAsync<ShelfRecordTracker>(command.ShelfRecordTrackerGuid);

            var trackedBook = tracker.GetTrackedBook(command.BookGuid, command.ShelfGuid);

            tracker.UnTrackBook(trackedBook);

            await _unitOfWork.CommitAsync(tracker);
        }
    }
}