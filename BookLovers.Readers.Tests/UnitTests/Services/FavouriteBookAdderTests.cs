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
    public class FavouriteBookAdderTests
    {
        private FavouriteBookAdder _favouriteBookAdder;
        private Profile _profile;
        private Fixture _fixture;

        [Test]
        public void AddFavourite_WhenCalled_ShouldAddBookToManager()
        {
            var bookGuid = _fixture.Create<Guid>();

            var favouriteBook = new FavouriteBook(bookGuid);

            _favouriteBookAdder.AddFavourite(_profile, favouriteBook);

            var @events = _profile.GetUncommittedEvents();

            _profile.Favourites.Should().HaveCount(1);
            _profile.Favourites.Should().AllBeEquivalentTo(favouriteBook);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<FavouriteBookAdded>();
        }

        [Test]
        public void AddFavourite_WhenCalledAndProfileIsNotActive_ShouldThrowBussinesRuleNotMeetException()
        {
            _profile.ApplyChange(new ProfileArchived(_profile.Guid));

            Action act = () => _favouriteBookAdder.AddFavourite(_profile, new FavouriteBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddFavourite_WhenCalledAndFavouriteBookWouldBeDuplicated_ShouldThrowBusinessRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();
            var favouriteBook = new FavouriteBook(bookGuid);

            _favouriteBookAdder.AddFavourite(_profile, favouriteBook);

            Action act = () => _favouriteBookAdder.AddFavourite(_profile, favouriteBook);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Profile cannot have duplicated favourite");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _favouriteBookAdder = new FavouriteBookAdder();

            var profileGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();
            var joinedAt = _fixture.Create<DateTime>();

            _profile = new Profile(profileGuid, readerGuid, joinedAt);

            _profile.CommitEvents();
        }
    }
}