using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.BookReaders;
using BookLovers.Publication.Events.Authors;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Services
{
    [TestFixture]
    public class AuthorFactoryTests
    {
        private AuthorFactory _authorFactory;
        private AuthorData _data;
        private Fixture _fixture;

        [Test]
        public void Create_WhenCalledWithInvalidData_ShouldThrowBusinessRuleNotMeetException()
        {
            this._data = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    (Sex) ReflectionHelper.CreateInstance(typeof(Sex)))
                .WithDetails(
                    new LifeLength(
                        _fixture.Create<DateTime>(),
                        _fixture.Create<DateTime>()),
                    _fixture.Create<string>())
                .WithDescription(string.Empty, string.Empty, string.Empty)
                .WithGuid(_fixture.Create<Guid>())
                .AddedBy(new BookReader(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { -1 });

            Action act = () => _authorFactory.CreateAuthor(_data);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void Create_WhenCalled_ShouldReturnValidAuthor()
        {
            this._data = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    Sex.Female)
                .WithDetails(
                    new LifeLength(
                        _fixture.Create<DateTime>(),
                        _fixture.Create<DateTime>()),
                    _fixture.Create<string>())
                .WithDescription(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>())
                .WithGuid(_fixture.Create<Guid>())
                .AddedBy(new BookReader(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()))
                .WithBooks(new List<Guid> { _fixture.Create<Guid>() })
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Drama.Value });

            Author author = null;

            Action act = () => author = this._authorFactory.CreateAuthor(_data);
            act();
            act.Should().NotThrow<BusinessRuleNotMetException>();

            author.Should().NotBeNull();
            author.Books.Should().HaveCount(_data.AuthorBooks.Count);
            author.Books.Should().NotContainNulls();
            author.Books.Should().OnlyHaveUniqueItems();
            author.Books.Select(s => s.BookGuid).SequenceEqual(this._data.AuthorBooks).Should().BeTrue();
            author.Followers.Should().HaveCount(0);
            author.Basics.Sex.Value.Should().BePositive();
            author.Basics.Sex.Value.Should().Be((byte) this._data.Basics.Sex.Value);
            author.Basics.FullName.Should().NotBeNull();
            author.Description.AboutAuthor.Should().BeEquivalentTo(_data.Description.AboutAuthor);
            author.Description.AuthorWebsite.Should().BeEquivalentTo(_data.Description.WebSite);

            author.Description.DescriptionSource.Should()
                .BeEquivalentTo(_data.Description.DescriptionSource);

            author.Details.BirthDate.Should().Be(_data.Details.LifeLength.BirthDate);
            author.Details.DeathDate.Should().Be(_data.Details.LifeLength.DeathDate);

            var @events = author.GetUncommittedEvents();

            @events.Where(p => p is AuthorCreated).ToList().Should().HaveCount(1);
            @events.Where(p => p.GetType() == typeof(AuthorBookAdded)).ToList().Should()
                .HaveCount(_data.AuthorBooks.Count);
            @events.Where(p => p.GetType() == typeof(AuthorGenreAdded)).ToList().Should()
                .HaveCount(_data.Genres.Count);
        }

        [Test]
        public void Create_WhenCalledWithoutAuthorDetails_ShouldReturnAuthorWithoutDetails()
        {
            this._data = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    Sex.Female)
                .WithDetails(
                    new LifeLength(null, null),
                    string.Empty)
                .WithDescription(string.Empty, string.Empty, string.Empty)
                .WithGuid(_fixture.Create<Guid>())
                .AddedBy(new BookReader(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Drama.Value });

            Author author = null;

            Action act = () => author = _authorFactory.CreateAuthor(this._data);

            act.Should().NotThrow<BusinessRuleNotMetException>();
            author.Should().NotBeNull();
            author.Books.Should().HaveCount(_data.AuthorBooks.Count);
            author.Books.Should().NotContainNulls();
            author.Books.Should().OnlyHaveUniqueItems();
            author.Books.Select(s => s.BookGuid).SequenceEqual(this._data.AuthorBooks).Should().BeTrue();
            author.Basics.Sex.Value.Should().BePositive();
            author.Basics.Sex.Value.Should().Be(this._data.Basics.Sex.Value);
            author.Basics.FullName.Should().NotBeNull();

            author.Details.BirthDate.Should().BeNull();
            author.Details.DeathDate.Should().BeNull();
            author.Details.BirthPlace.Should().Be(_data.Details.BirthPlace);

            var @events = author.GetUncommittedEvents();

            @events.Where(p => p is AuthorCreated).ToList().Should().HaveCount(1);
            @events.Where(p => p.GetType() == typeof(AuthorBookAdded)).ToList().Should()
                .HaveCount(_data.AuthorBooks.Count);
            @events.Where(p => p.GetType() == typeof(AuthorGenreAdded)).ToList().Should()
                .HaveCount(_data.Genres.Count);
        }

        [Test]
        public void Create_WhenCalledWithoutGenres_ShouldReturnAuthorWithoutGenres()
        {
            this._data = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    Sex.Male)
                .WithDetails(
                    new LifeLength(
                        _fixture.Create<DateTime>(),
                        _fixture.Create<DateTime>()),
                    _fixture.Create<string>())
                .WithDescription(string.Empty, string.Empty, string.Empty)
                .WithGuid(_fixture.Create<Guid>())
                .AddedBy(new BookReader(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int>());

            Author author = null;

            Action act = () => author = _authorFactory.CreateAuthor(this._data);

            act.Should().NotThrow<BusinessRuleNotMetException>();
            author.Should().NotBeNull();
            author.Books.Should().HaveCount(_data.AuthorBooks.Count);
            author.Books.Should().NotContainNulls();
            author.Books.Should().OnlyHaveUniqueItems();
            author.Books.Select(s => s.BookGuid).SequenceEqual(this._data.AuthorBooks).Should().BeTrue();
            author.Followers.Should().HaveCount(0);
            author.Basics.Sex.Value.Should().BePositive();
            author.Basics.Sex.Value.Should().Be((byte) this._data.Basics.Sex.Value);
            author.Basics.FullName.Should().NotBeNull();

            var @events = author.GetUncommittedEvents();

            @events.Where(p => p is AuthorCreated).ToList().Should().HaveCount(1);
            @events.Where(p => p.GetType() == typeof(AuthorBookAdded)).ToList().Should()
                .HaveCount(_data.AuthorBooks.Count);
            @events.Where(p => p.GetType() == typeof(AuthorGenreAdded)).ToList().Should()
                .HaveCount(_data.Genres.Count);
        }

        [Test]
        public void Create_WhenCalledWithoutAuthorDescription_ShouldReturnAuthorWithoutDescription()
        {
            this._data = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    Sex.Female)
                .WithDetails(
                    new LifeLength(
                        _fixture.Create<DateTime>(),
                        _fixture.Create<DateTime>()),
                    _fixture.Create<string>())
                .WithDescription(null, null, null)
                .WithGuid(_fixture.Create<Guid>())
                .AddedBy(new BookReader(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Drama.Value });

            Author author = null;

            Action act = () => author = this._authorFactory.CreateAuthor(_data);

            act.Should().NotThrow<BusinessRuleNotMetException>();
            author.Should().NotBeNull();
            author.Books.Should().HaveCount(_data.AuthorBooks.Count);
            author.Books.Should().NotContainNulls();
            author.Books.Should().OnlyHaveUniqueItems();
            author.Books.Select(s => s.BookGuid).SequenceEqual(this._data.AuthorBooks).Should().BeTrue();
            author.Basics.Sex.Value.Should().BePositive();
            author.Basics.Sex.Value.Should().Be((byte) this._data.Basics.Sex.Value);
            author.Basics.FullName.Should().NotBeNull();

            author.Description.AboutAuthor.Should().BeNull();
            author.Description.AuthorWebsite.Should().BeNull();
            author.Description.DescriptionSource.Should()
                .BeNull();

            var @events = author.GetUncommittedEvents();
            @events.Where(p => p is AuthorCreated).ToList().Should().HaveCount(1);
            @events.Where(p => p.GetType() == typeof(AuthorBookAdded)).ToList().Should()
                .HaveCount(_data.AuthorBooks.Count);
            @events.Where(p => p.GetType() == typeof(AuthorGenreAdded)).ToList().Should()
                .HaveCount(_data.Genres.Count);
        }

        [Test]
        public void Create_WhenCalledWithoutBooks_ShouldCreateAuthorWithoutBooks()
        {
            this._data = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    Sex.Female)
                .WithDetails(
                    new LifeLength(
                        _fixture.Create<DateTime>(),
                        _fixture.Create<DateTime>()),
                    _fixture.Create<string>())
                .WithDescription(
                    _fixture.Create<string>(),
                    _fixture.Create<string>(), _fixture.Create<string>())
                .WithGuid(_fixture.Create<Guid>())
                .AddedBy(new BookReader(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()))
                .WithBooks(new List<Guid>())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Drama.Value });

            Author author = null;

            Action act = () => author = this._authorFactory.CreateAuthor(_data);

            act.Should().NotThrow<BusinessRuleNotMetException>();
            author.Should().NotBeNull();
            author.Books.Should().HaveCount(_data.AuthorBooks.Count);
            author.Books.Should().NotContainNulls();
            author.Books.Should().OnlyHaveUniqueItems();
            author.Books.Select(s => s.BookGuid).SequenceEqual(this._data.AuthorBooks).Should().BeTrue();
            author.Basics.Sex.Value.Should().BePositive();
            author.Basics.Sex.Value.Should().Be((byte) this._data.Basics.Sex.Value);
            author.Basics.FullName.Should().NotBeNull();

            var @events = author.GetUncommittedEvents();
            @events.Where(p => p is AuthorCreated).ToList().Should().HaveCount(1);
            @events.Where(p => p.GetType() == typeof(AuthorBookAdded)).ToList().Should()
                .HaveCount(_data.AuthorBooks.Count);
            @events.Where(p => p.GetType() == typeof(AuthorGenreAdded)).ToList().Should()
                .HaveCount(_data.Genres.Count);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _fixture.Create<Guid>();
            var authorBuilder = _fixture.Create<AuthorBuilder>();
            var mock = new Mock<IAuthorUniquenessChecker>();
            mock.Setup(s => s.IsUnique(It.IsAny<Guid>())).Returns(true);

            _authorFactory = new AuthorFactory(authorBuilder, mock.Object);
        }
    }
}