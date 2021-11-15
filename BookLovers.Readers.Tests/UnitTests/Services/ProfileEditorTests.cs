using System;
using System.Linq;
using AutoFixture;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;
using BookLovers.Readers.Domain.Profiles.Services.Factories;
using BookLovers.Readers.Events.Profile;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class ProfileEditorTests
    {
        private ProfileEditor _profileEditor;
        private Profile _profile;
        private ProfileData _profileData;
        private Fixture _fixture;

        [Test]
        public void EditProfile_WhenCalled_ShouldEditProfile()
        {
            var oldJoinedAt = _profile.About.JoinedAt;
            _profileEditor.EditProfile(_profile, _profileData);

            var @events = _profile.GetUncommittedEvents();
            @events.Should().HaveCount(3);
            @events.First().Should().BeAssignableTo<AddressChanged>();
            @events.Skip(1).First().Should().BeAssignableTo<IdentityChanged>();
            @events.Skip(2).First().Should().BeAssignableTo<ProfileAboutChanged>();

            _profile.About.AboutYourself.Should().Be(_profileData.DetailsData.AboutUser);
            _profile.About.WebSite.Should().Be(_profileData.DetailsData.WebSite);
            _profile.About.JoinedAt.Should().Be(oldJoinedAt);
            _profile.Address.City.Should().Be(_profileData.DetailsData.City);
            _profile.Address.Country.Should().BeEquivalentTo(_profileData.DetailsData.Country);
            _profile.Identity.Sex.Should().Be(_profileData.ContentData.Sex);
            _profile.Identity.BirthDate.Should().Be(_profileData.ContentData.BirthDate);
            _profile.Identity.FullName.Should().Be(_profileData.ContentData.FullName);
        }

        [Test]
        public void EditProfile_WhenCalledWithSameData_ShouldNotEmitAnyChanges()
        {
            _profileEditor.EditProfile(_profile, _profileData);

            _profile.CommitEvents();

            _profileEditor.EditProfile(_profile, _profileData);

            var @events = _profile.GetUncommittedEvents();

            @events.Should().HaveCount(0);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _profileEditor = new ProfileEditor();

            var profileGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();
            var joinedAt = _fixture.Create<DateTime>();

            _profile = new Profile(profileGuid, readerGuid, joinedAt);

            _profile.CommitEvents();

            var detailsData = ProfileDetailsData.Initialize()
                .WithCity(_fixture.Create<string>())
                .WithCountry(_fixture.Create<string>())
                .WithAboutUser(_fixture.Create<string>())
                .WithWebSite(_fixture.Create<string>());

            var contentData = new ProfileContentData(
                new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                _fixture.Create<DateTime>(), Sex.Female);

            _profileData = ProfileData.Initialize()
                .WithContent(contentData)
                .WithDetails(detailsData);
        }
    }
}