using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.CommandHandlers.Statistics
{
    internal class CreateStatisticsGathererHandler :
        ICommandHandler<CreateStatisticsGathererInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateStatisticsGathererHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateStatisticsGathererInternalCommand command)
        {
            var gatherer = new StatisticsGatherer(command.StatisticsGathererGuid, command.ReaderGuid,
                command.ProfileGuid);

            return _unitOfWork.CommitAsync(gatherer);
        }
    }
}