using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Domain.ShelfRecordTracker;

namespace BookLovers.Bookcases.Application.CommandHandlers.ShelfTrackers
{
    internal class ArchiveShelfRecordTrackerHandler :
        ICommandHandler<ArchiveShelfRecordTrackerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<ShelfRecordTracker> _aggregateManager;

        public ArchiveShelfRecordTrackerHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<ShelfRecordTracker> aggregateManager)
        {
            _unitOfWork = unitOfWork;
            _aggregateManager = aggregateManager;
        }

        public async Task HandleAsync(ArchiveShelfRecordTrackerInternalCommand command)
        {
            var recordTracker = await _unitOfWork.GetAsync<ShelfRecordTracker>(command.ShelfRecordTrackerGuid);

            _aggregateManager.Archive(recordTracker);

            await _unitOfWork.CommitAsync(recordTracker);
        }
    }
}