using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands;
using BookLovers.Bookcases.Domain.Settings;

namespace BookLovers.Bookcases.Application.CommandHandlers.SettingsManagers
{
    internal class CreateSettingsManagerHandler : ICommandHandler<CreateSettingsManagerInternalCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSettingsManagerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task HandleAsync(CreateSettingsManagerInternalCommand command)
        {
            var manager = new SettingsManager(command.SettingsManagerGuid, command.BookcaseGuid);

            return _unitOfWork.CommitAsync(manager);
        }
    }
}