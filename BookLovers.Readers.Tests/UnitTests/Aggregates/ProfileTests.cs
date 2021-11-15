using System;
using System.Collections.Generic;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Domain.Profiles.Services;
using BookLovers.Readers.Events.Profile;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class ProfileTests
    {
        private Profile _profile;
        private FavouritesService _favouritesService;
        private Fixture _fixture;

        [Test]
        [TestCase("SOME_COUNTRY", "SOME_CITY")]
        [TestCase("ABC", "ABC")]
        public void ChangeAddress_WhenCalled_ShouldChangeReadersAddress(string country, string city)
        {
            _profile.ChangeAddress(new Address(country, city));

            var @events = _profile.GetUncommittedEvents();

            _profile.Address.City.Should().Be(city);
            _profile.Address.Country.Should().Be(country);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<AddressChanged>();
        }

        [Test]
        public void ChangeAddress_WhenProfileIsInActive_ShouldThrowBusinessRuleNotMeetException()
        {
            _profile.ApplyChange(new ProfileArchived(_profile.Guid));

            Action act = () => _profile.ChangeAddress(new Address("SOME_COUNTRY", "SOME_CITY"));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        [TestCase("SOME FULLNAME", 1, "2014-12-12")]
        [TestCase("John Doe", 2, "2012-04-04")]
        [TestCase("Hello Kitty", 3, "2010-01-01")]
        [TestCase("", 2, "1992-12-12")]
        public void ChangeIdentity_WhenCalled_ShouldChangeSocialProfileIdentity(string fullName, byte sex,
            DateTime birthDate)
        {
            _profile.ChangeIdentity(new Identity(fullName, sex, birthDate));

            var @events = _profile.GetUncommittedEvents();

            _profile.Identity.FullName.Should().Be(new FullName(fullName));
            _profile.Identity.Sex.Value.Should().Be(sex);
            _profile.Identity.BirthDate.Should().Be(birthDate);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<IdentityChanged>();
        }

        [Test]
        public void ChangeIdentity_WhenCalledWithInActiveProfile_ShouldThrowBusinessRuleNotMeetException()
        {
            _profile.ApplyChange(new ProfileArchived(_profile.Guid));

            Action act = () =>
                _profile.ChangeIdentity(new Identity("SOME_FULLNAME", Sex.Female.Value, DateTime.UtcNow));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        [TestCase(255)]
        [TestCase(4)]
        public void ChangeIdentity_WhenCalledWithInvalidSex_ShouldThrowBusinessRuleNotMeetException(byte sex)
        {
            Action act = () => _profile.ChangeIdentity(new Identity("SOME_FULLNAME", sex, DateTime.UtcNow));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Reader sex is not valid.");
        }

        [Test]
        [TestCase("WEB_SITE", "SOME_ABOUT")]
        [TestCase("ABC", "ABC")]
        public void ChangeAbout_WhenCalled_ShouldChangeAbout(string webSite, string about)
        {
            _profile.ChangeAbout(new About(DateTime.UtcNow, webSite, about));

            var @events = _profile.GetUncommittedEvents();

            _profile.About.AboutYourself.Should().Be(about);
            _profile.About.WebSite.Should().Be(webSite);

            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<ProfileAboutChanged>();
        }

        [Test]
        public void ChangeAbout_WhenCalledWithInActiveProfile_ShouldThrowBusinessRuleNotMeetException()
        {
            _profile.ApplyChange(new ProfileArchived(_profile.Guid));

            Action act = () => _profile.ChangeAbout(new About(DateTime.UtcNow, "WEB_SITE", "ABOUT"));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveFavourite_WhenCalled_ShouldRemoveSelectedFavourite()
        {
            var favouriteBookGuid = _fixture.Create<Guid>();

            _favouritesService.AddFavourite(_profile, new FavouriteBook(favouriteBookGuid));

            _profile.CommitEvents();

            var favouriteBook = _profile.GetFavourite(favouriteBookGuid);

            _favouritesService.RemoveFavourite(_profile, favouriteBook);

            _profile.Favourites.Should().NotContain(favouriteBook);
            _profile.Favourites.Count.Should().Be(0);
        }

        [Test]
        public void RemoveFavourite_WhenCalledAndProfileIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var favouriteBookGuid = _fixture.Create<Guid>();

            _favouritesService.AddFavourite(_profile, new FavouriteBook(favouriteBookGuid));

            _profile.CommitEvents();

            var favouriteBook = _profile.GetFavourite(favouriteBookGuid);

            _profile.ApplyChange(new ProfileArchived(_profile.Guid));

            Action act = () => _favouritesService.RemoveFavourite(_profile, favouriteBook);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveFavourite_WhenCalledAndSelectedFavouriteIsNotPresent_ShouldThrowBusinessRuleNotMeetException()
        {
            var favouriteBookGuid = _fixture.Create<Guid>();

            _favouritesService.AddFavourite(_profile, new FavouriteBook(favouriteBookGuid));

            _profile.CommitEvents();

            var favouriteBook = _profile.GetFavourite(favouriteBookGuid);

            _favouritesService.RemoveFavourite(_profile, favouriteBook);

            Action act = () => _favouritesService.RemoveFavourite(_profile, favouriteBook);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Profile must have selected favourite.");
        }

        [Test]
        public void GetFavourite_WhenCalled_ShouldReturnSelectedFavourite()
        {
            var favouriteBookGuid = _fixture.Create<Guid>();

            _favouritesService.AddFavourite(_profile, new FavouriteBook(favouriteBookGuid));

            var favouriteBook = _profile.GetFavourite(favouriteBookGuid);

            favouriteBook.Should().NotBeNull();
            favouriteBook.FavouriteGuid.Should().Be(favouriteBookGuid);
        }

        [Test]
        public void GetFavourite_WhenCalled_ShouldReturnNull()
        {
            var favourite = _profile.GetFavourite(_fixture.Create<Guid>());

            favourite.Should().BeNull();
        }

        [Test]
        public void HasFavourite_WhenCalled_ShouldReturnTrue()
        {
            var favouriteBookGuid = _fixture.Create<Guid>();

            _favouritesService.AddFavourite(_profile, new FavouriteBook(favouriteBookGuid));

            var result = _profile.HasFavourite(favouriteBookGuid);

            result.Should().BeTrue();
        }

        [Test]
        public void HasFavourite_WhenCalled_ShouldReturnFalse()
        {
            var result = _profile.HasFavourite(_fixture.Create<Guid>());

            result.Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            var profileGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();
            var joinedAt = _fixture.Create<DateTime>();

            _profile = new Profile(profileGuid, readerGuid, joinedAt);

            _profile.CommitEvents();

            var authorAdder = new FavouriteAuthorAdder();
            var bookAdder = new FavouriteBookAdder();
            var authorRemover = new FavouriteAuthorRemover();
            var bookRemover = new FavouriteBookRemover();

            var adders = new Dictionary<FavouriteType, IFavouriteAdder>
            {
                { FavouriteType.FavouriteAuthor, authorAdder },
                { FavouriteType.FavouriteBook, bookAdder },
            };

            var removers = new Dictionary<FavouriteType, IFavouriteRemover>()
            {
                { FavouriteType.FavouriteAuthor, authorRemover },
                { FavouriteType.FavouriteBook, bookRemover }
            };

            _favouritesService = new FavouritesService(adders, removers);
        }
    }
}