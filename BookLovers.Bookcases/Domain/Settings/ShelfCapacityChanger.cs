namespace BookLovers.Bookcases.Domain.Settings
{
    internal class ShelfCapacityChanger : ISettingsChanger
    {
        public BookcaseOptionType OptionType => BookcaseOptionType.ShelfCapacity;

        public void ChangeSetting(SettingsManager settingsManager, int selectedOption)
        {
            var shelfCapacity = new ShelfCapacity(selectedOption);

            settingsManager.GetOption(BookcaseOptionType.ShelfCapacity);

            settingsManager.ChangeCapacity(selectedOption);
        }
    }
}