using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Bookcases.Domain.Settings
{
    public class SettingsChanger : IDomainService<SettingsManager>
    {
        private readonly IDictionary<BookcaseOptionType, ISettingsChanger> _changers;

        public SettingsChanger(IDictionary<BookcaseOptionType, ISettingsChanger> changers)
        {
            _changers = changers;
        }

        public void ChangeSettings(
            SettingsManager settingsManager,
            SelectedBookcaseOption selectedBookcaseOption)
        {
            if (settingsManager == null || !settingsManager.IsActive())
                throw new BusinessRuleNotMetException("Selected bookcase does not exist");

            _changers[selectedBookcaseOption.OptionType]
                .ChangeSetting(settingsManager, selectedBookcaseOption.Value);
        }
    }
}