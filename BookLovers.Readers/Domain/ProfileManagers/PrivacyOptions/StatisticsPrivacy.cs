using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public class StatisticsPrivacy : BookLovers.Base.Domain.ValueObject.ValueObject<StatisticsPrivacy>, IPrivacyOption
    {
        public ProfilePrivacyType PrivacyType => ProfilePrivacyType.StatisticsPrivacy;

        public PrivacyOption PrivacyOption { get; }

        private StatisticsPrivacy()
        {
        }

        public StatisticsPrivacy(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public IPrivacyOption ChangeTo(int privacyOptionId)
        {
            return new StatisticsPrivacy(AvailablePrivacyOptions.Get(privacyOptionId));
        }

        public IPrivacyOption DefaultOption()
        {
            return new StatisticsPrivacy(PrivacyOption.Public);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + PrivacyOption.GetHashCode();
            hash = (hash * 23) + PrivacyType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(StatisticsPrivacy obj)
        {
            return PrivacyOption.Value == obj.PrivacyOption.Value &&
                   PrivacyType.Value == obj.PrivacyType.Value;
        }
    }
}