using BookLovers.Base.Domain.Extensions;
using Newtonsoft.Json;

namespace BookLovers.Readers.Domain.ProfileManagers.Services
{
    public class ProfilePrivacyType : Enumeration
    {
        public static readonly ProfilePrivacyType ProfilePrivacy = new ProfilePrivacyType(1, nameof(ProfilePrivacy));

        public static readonly ProfilePrivacyType IdentityPrivacy = new ProfilePrivacyType(2, nameof(IdentityPrivacy));

        public static readonly ProfilePrivacyType GenderPrivacy = new ProfilePrivacyType(3, nameof(GenderPrivacy));

        public static readonly ProfilePrivacyType AddressPrivacy = new ProfilePrivacyType(4, nameof(AddressPrivacy));

        public static readonly ProfilePrivacyType FavouritesPrivacy =
            new ProfilePrivacyType(5, nameof(FavouritesPrivacy));

        public static readonly ProfilePrivacyType StatisticsPrivacy =
            new ProfilePrivacyType(6, nameof(StatisticsPrivacy));

        [JsonConstructor]
        protected ProfilePrivacyType(byte value, string name)
            : base(value, name)
        {
        }
    }
}