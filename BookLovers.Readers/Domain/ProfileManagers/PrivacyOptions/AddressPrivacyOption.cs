﻿using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions
{
    public class AddressPrivacyOption : BookLovers.Base.Domain.ValueObject.ValueObject<AddressPrivacyOption>,
        IPrivacyOption
    {
        public ProfilePrivacyType PrivacyType => ProfilePrivacyType.AddressPrivacy;

        public PrivacyOption PrivacyOption { get; }

        private AddressPrivacyOption()
        {
        }

        public AddressPrivacyOption(PrivacyOption privacyOption)
        {
            PrivacyOption = privacyOption;
        }

        public IPrivacyOption ChangeTo(int privacyOptionId)
        {
            return new AddressPrivacyOption(AvailablePrivacyOptions.Get(privacyOptionId));
        }

        public IPrivacyOption DefaultOption()
        {
            return new AddressPrivacyOption(PrivacyOption.Public);
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + PrivacyOption.GetHashCode();
            hash = (hash * 23) + PrivacyType.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AddressPrivacyOption obj)
        {
            return PrivacyOption.Value == obj.PrivacyOption.Value && PrivacyType.Value == obj.PrivacyType.Value;
        }
    }
}