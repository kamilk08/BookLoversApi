using BookLovers.Base.Domain.ValueObject;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public class GenderPrivacyOption : ValueObject<GenderPrivacyOption>, IPrivacyOption
    {
        public ProfilePrivacyType PrivacyType => ProfilePrivacyType.GenderPrivacy;

        public PrivacyOption PrivacyOption { get; }

        private GenderPrivacyOption()
        {
        }

        public GenderPrivacyOption(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public IPrivacyOption ChangeTo(int privacyOptionId)
        {
            return new GenderPrivacyOption(AvailablePrivacyOptions.Get(privacyOptionId));
        }

        public IPrivacyOption DefaultOption()
        {
            return new GenderPrivacyOption(PrivacyOption.Public);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + PrivacyType.GetHashCode();
            hash = (hash * 23) + PrivacyOption.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(GenderPrivacyOption obj)
        {
            return PrivacyOption == obj.PrivacyOption &&
                   PrivacyType == obj.PrivacyType;
        }
    }
}