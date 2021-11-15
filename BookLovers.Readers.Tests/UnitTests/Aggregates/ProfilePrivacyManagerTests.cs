using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.ProfileManagers;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Readers.Events.ProfileManagers;
using BookLovers.Shared.Privacy;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class ProfilePrivacyManagerTests
    {
        private Fixture _fixture;
        private ProfilePrivacyManager _privacyManager;

        [Test]
        public void ChangePrivacy_WhenCalled_ShouldChangePrivacyOption()
        {
            _privacyManager.ChangePrivacy(new SelectedProfileOption(
                PrivacyOption.Public,
                ProfilePrivacyType.AddressPrivacy));

            var addressPrivacy = _privacyManager.GetPrivacyOption(ProfilePrivacyType.AddressPrivacy);

            addressPrivacy.PrivacyOption.Should().Be(PrivacyOption.Public);
        }

        [Test]
        public void ChangePrivacy_WhenCalledAndAggregateIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _privacyManager.ApplyChange(new ProfilePrivacyManagerArchived(_fixture.Create<Guid>()));

            Action act = () =>
                _privacyManager.ChangePrivacy(new SelectedProfileOption(
                    PrivacyOption.Public,
                    ProfilePrivacyType.AddressPrivacy));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ChangePrivacy_WhenCalledAndPrivacyOptionIsNotValid_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () =>
                _privacyManager.ChangePrivacy(new SelectedProfileOption(10, ProfilePrivacyType.GenderPrivacy.Value));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        public void ChangePrivacy_WhenCalledAndPrivacyTypeIsNotValid_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => _privacyManager.ChangePrivacy(new SelectedProfileOption(PrivacyOption.Public.Value, 20));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _privacyManager = new ProfilePrivacyManager(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            var addressPrivacyOptionAdded = ProfilePrivacyOptionAdded.Initialize()
                .WithAggregate(_privacyManager.Guid)
                .WithOption(PrivacyOption.Public.Value, PrivacyOption.Public.Name)
                .WithPrivacyType(ProfilePrivacyType.AddressPrivacy.Value, ProfilePrivacyType.AddressPrivacy.Name);

            var favouritesPrivacyOptionAdded = ProfilePrivacyOptionAdded.Initialize()
                .WithAggregate(_privacyManager.Guid)
                .WithOption(PrivacyOption.Public.Value, PrivacyOption.Public.Name)
                .WithPrivacyType(ProfilePrivacyType.FavouritesPrivacy.Value, ProfilePrivacyType.FavouritesPrivacy.Name);

            var genderPrivacyOptionAdded = ProfilePrivacyOptionAdded.Initialize()
                .WithAggregate(_privacyManager.Guid)
                .WithOption(PrivacyOption.Public.Value, PrivacyOption.Public.Name)
                .WithPrivacyType(ProfilePrivacyType.GenderPrivacy.Value, ProfilePrivacyType.GenderPrivacy.Name);

            var identityPrivacyOptionAdded = ProfilePrivacyOptionAdded.Initialize()
                .WithAggregate(_privacyManager.Guid)
                .WithOption(PrivacyOption.Public.Value, PrivacyOption.Public.Name)
                .WithPrivacyType(ProfilePrivacyType.AddressPrivacy.Value, ProfilePrivacyType.IdentityPrivacy.Name);

            _privacyManager.ApplyChange(addressPrivacyOptionAdded);
            _privacyManager.ApplyChange(favouritesPrivacyOptionAdded);
            _privacyManager.ApplyChange(genderPrivacyOptionAdded);
            _privacyManager.ApplyChange(identityPrivacyOptionAdded);
        }
    }
}