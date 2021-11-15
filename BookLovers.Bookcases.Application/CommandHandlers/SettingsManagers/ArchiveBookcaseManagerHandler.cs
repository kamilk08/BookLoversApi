using System.Threading.Tasks;
using BookLovers.Base.Domain.Services;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands;
using BookLovers.Bookcases.Domain.Settings;

namespace BookLovers.Bookcases.Application.CommandHandlers.SettingsManagers
{
    internal class ArchiveBookcaseManagerHandler :
        ICommandHandler<ArchiveSettingsManagerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAggregateManager<SettingsManager> _aggregateManager;

        public ArchiveBookcaseManagerHandler(
            IUnitOfWork unitOfWork,
            IAggregateManager<SettingsManager> aggregateManager)
        {
            _unitOfWork = unitOfWork;
            _aggregateManager = aggregateManager;
        }

        public async Task HandleAsync(ArchiveSettingsManagerInternalCommand command)
        {
            var settingsManager = await _unitOfWork.GetAsync<SettingsManager>(command.SettingsManagerGuid);

            _aggregateManager.Archive(settingsManager);

            await _unitOfWork.CommitAsync(settingsManager);
        }
    }
}