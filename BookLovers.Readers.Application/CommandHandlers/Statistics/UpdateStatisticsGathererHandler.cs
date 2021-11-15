using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Readers.Application.Commands.Statistics;
using BookLovers.Readers.Domain.Readers;
using BookLovers.Readers.Domain.Statistics;

namespace BookLovers.Readers.Application.CommandHandlers.Statistics
{
    internal class UpdateStatisticsGathererHandler :
        ICommandHandler<UpdateStatisticsGathererInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateStatisticsGathererHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task HandleAsync(UpdateStatisticsGathererInternalCommand command)
        {
            var reader = await _unitOfWork.GetAsync<Reader>(command.ReaderGuid);

            var gatherer = await _unitOfWork.GetAsync<StatisticsGatherer>(reader.Socials.StatisticsGathererGuid);

            gatherer.ChangeStatistic(command.StatisticsType, command.StatisticsStep);

            await _unitOfWork.CommitAsync(gatherer);
        }
    }
}