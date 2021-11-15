using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public class IdentityPrivacyOption : BookLovers.Base.Domain.ValueObject.ValueObject<IdentityPrivacyOption>,
        IPrivacyOption
    {
        public ProfilePrivacyType PrivacyType => ProfilePrivacyType.IdentityPrivacy;

        public PrivacyOption PrivacyOption { get; }

        private IdentityPrivacyOption()
        {
        }

        public IdentityPrivacyOption(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public IPrivacyOption ChangeTo(int privacyOptionId)
        {
            return new IdentityPrivacyOption(AvailablePrivacyOptions.Get(privacyOptionId));
        }

        public IPrivacyOption DefaultOption()
        {
            return new IdentityPrivacyOption(PrivacyOption.Public);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + PrivacyOption.GetHashCode();
            hash = (hash * 23) + PrivacyType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(IdentityPrivacyOption obj)
        {
            return PrivacyOption.Value == obj.PrivacyOption.Value &&
                   PrivacyType.Value == obj.PrivacyType.Value;
        }
    }
}