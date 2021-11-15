using BookLovers.Base.Domain.ValueObject;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public class FavouritePrivacyOption : ValueObject<FavouritePrivacyOption>, IPrivacyOption
    {
        public ProfilePrivacyType PrivacyType => ProfilePrivacyType.FavouritesPrivacy;

        public PrivacyOption PrivacyOption { get; }

        private FavouritePrivacyOption()
        {
        }

        public FavouritePrivacyOption(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public IPrivacyOption ChangeTo(int privacyOptionId)
        {
            return new FavouritePrivacyOption(AvailablePrivacyOptions.Get(privacyOptionId));
        }

        public IPrivacyOption DefaultOption()
        {
            return new FavouritePrivacyOption(PrivacyOption.Public);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + PrivacyOption.GetHashCode();
            hash = (hash * 23) + PrivacyType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(FavouritePrivacyOption obj)
        {
            return PrivacyOption.Value == obj.PrivacyOption.Value &&
                   PrivacyType.Value == obj.PrivacyType.Value;
        }
    }
}