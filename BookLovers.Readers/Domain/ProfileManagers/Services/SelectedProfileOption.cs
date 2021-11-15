using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.Services
{
    public class SelectedProfileOption
    {
        public PrivacyOption PrivacyOption { get; set; }

        public ProfilePrivacyType PrivacyType { get; set; }

        public SelectedProfileOption(int selectedOptionId, int privacyTypeId)
        {
            PrivacyOption = AvailablePrivacyOptions.Get(selectedOptionId);
            PrivacyType = ProfilePrivates.Get(privacyTypeId);
        }

        public SelectedProfileOption(PrivacyOption privacyOption, ProfilePrivacyType privacyType)
        {
            PrivacyOption = privacyOption;
            PrivacyType = privacyType;
        }
    }
}