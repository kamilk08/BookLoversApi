using System;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;
using BookLovers.Readers.Events.Profile;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Services
{
    [TestFixture]
    public class FavouriteAuthorAdderTests
    {
        private FavouriteAuthorAdder _authorAdder;
        private Profile _profile;
        private Fixture _fixture;

        [Test]
        public void AddFavourite_WhenCalled_ShouldAddFavouriteAuthor()
        {
            var authorGuid = _fixture.Create<Guid>();

            var favouriteAuthor = new FavouriteAuthor(authorGuid);

            _authorAdder.AddFavourite(_profile, favouriteAuthor);

            var @events = _profile.GetUncommittedEvents();

            _profile.Favourites.Should().HaveCount(1);
            _profile.Favourites.Should().AllBeEquivalentTo(favouriteAuthor);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<FavouriteAuthorAdded>();
        }

        [Test]
        public void AddFavourite_WhenCalledAndProfileIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _profile.ApplyChange(new ProfileArchived(_profile.Guid));

            Action act = () => _authorAdder.AddFavourite(_profile, new FavouriteAuthor(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void
            AddFavourite_WhenCalledAndThereIsAlreadySameFavouriteAuthor_ShouldThrowBussinesRuleNotMeetException()
        {
            var authorGuid = _fixture.Create<Guid>();
            var favouriteAuthor = new FavouriteAuthor(authorGuid);

            _authorAdder.AddFavourite(_profile, favouriteAuthor);

            Action act = () => _authorAdder.AddFavourite(_profile, favouriteAuthor);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Profile cannot have duplicated favourite");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _authorAdder = new FavouriteAuthorAdder();

            var profileGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();
            var joinedAt = _fixture.Create<DateTime>();

            _profile = new Profile(profileGuid, readerGuid, joinedAt);

            _profile.CommitEvents();
        }
    }
}