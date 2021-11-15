using BookLovers.Shared.Privacy;

namespace BookLovers.Bookcases.Domain.Settings
{
    internal class PrivacyChanger : ISettingsChanger
    {
        public BookcaseOptionType OptionType => BookcaseOptionType.Privacy;

        public void ChangeSetting(SettingsManager settingsManager, int selectedOption)
        {
            var privacy = new BookcasePrivacy(AvailablePrivacyOptions.Get(selectedOption));

            settingsManager.GetOption(BookcaseOptionType.Privacy);

            settingsManager.ChangePrivacy(privacy);
        }
    }
}