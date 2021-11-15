using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public interface IPrivacyOption
    {
        ProfilePrivacyType PrivacyType { get; }

        PrivacyOption PrivacyOption { get; }

        IPrivacyOption ChangeTo(int privacyOptionId);

        IPrivacyOption DefaultOption();
    }
}