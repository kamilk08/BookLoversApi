namespace BookLovers.Bookcases.Domain.Settings
{
    public interface ISettingsChanger
    {
        BookcaseOptionType OptionType { get; }

        void ChangeSetting(SettingsManager settingsManager, int selectedOption);
    }
}