using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.ShelfTrackers;
using BookLovers.Bookcases.Domain.ShelfRecordTracker;

namespace BookLovers.Bookcases.Application.CommandHandlers.ShelfTrackers
{
    internal class CreateShelfRecordTrackerHandler :
        ICommandHandler<CreateShelfRecordTrackerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateShelfRecordTrackerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task HandleAsync(
            CreateShelfRecordTrackerInternalCommand command)
        {
            var tracker = new ShelfRecordTracker(command.ShelfRecordTrackerGuid, command.BookcaseGuid);

            return _unitOfWork.CommitAsync(tracker);
        }
    }
}