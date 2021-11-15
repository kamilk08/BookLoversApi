using System.Threading.Tasks;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Commands;
using BookLovers.Bookcases.Application.Commands.Bookcases;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.Settings;

namespace BookLovers.Bookcases.Application.CommandHandlers.SettingsManagers
{
    internal class ChangeOptionsHandler : ICommandHandler<ChangeBookcaseOptionsCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SettingsChanger _settingsChanger;

        public ChangeOptionsHandler(IUnitOfWork unitOfWork, SettingsChanger settingsChanger)
        {
            _unitOfWork = unitOfWork;
            _settingsChanger = settingsChanger;
        }

        public async Task HandleAsync(ChangeBookcaseOptionsCommand command)
        {
            var bookcase = await _unitOfWork.GetAsync<Bookcase>(command.WriteModel.BookcaseGuid);

            var settingsManager = await _unitOfWork.GetAsync<SettingsManager>(
                bookcase.Additions.SettingsManagerGuid);

            command.WriteModel.SelectedOptions.ForEach(selectedOption =>
                _settingsChanger.ChangeSettings(
                    settingsManager,
                    new SelectedBookcaseOption(selectedOption.Value, selectedOption.OptionType)));

            await _unitOfWork.CommitAsync(settingsManager);
        }
    }
}