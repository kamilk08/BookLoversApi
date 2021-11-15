using BookLovers.Base.Domain.ValueObject;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public class ProfilePrivacy : ValueObject<ProfilePrivacy>, IPrivacyOption
    {
        public ProfilePrivacyType PrivacyType => ProfilePrivacyType.ProfilePrivacy;

        public PrivacyOption PrivacyOption { get; }

        private ProfilePrivacy()
        {
        }

        public ProfilePrivacy(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public IPrivacyOption ChangeTo(int privacyOptionId)
        {
            return new ProfilePrivacy(AvailablePrivacyOptions.Get(privacyOptionId));
        }

        public IPrivacyOption DefaultOption()
        {
            return new ProfilePrivacy(PrivacyOption.Public);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + PrivacyOption.GetHashCode();
            hash = (hash * 23) + PrivacyType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(ProfilePrivacy obj)
        {
            return PrivacyOption.Value == obj.PrivacyOption.Value &&
                   PrivacyType.Value == obj.PrivacyType.Value;
        }
    }
}