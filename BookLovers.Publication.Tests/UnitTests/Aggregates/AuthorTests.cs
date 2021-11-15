using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Events.Authors;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Aggregates
{
    [TestFixture]
    public class AuthorTests
    {
        private Author _author;
        private Guid _followerGuid;
        private Fixture _fixture;

        [Test]
        public void AddBook_WhenCalled_ShouldAddBookToAuthorsCollection()
        {
            var bookGuid = _fixture.Create<Guid>();

            _author.AddBook(new AuthorBook(bookGuid));

            _author.Books.Should().HaveCount(1);
            _author.Books.Should().Contain(p => p.BookGuid == bookGuid);

            var @events = _author.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().Contain(p => p.GetType() == typeof(AuthorBookAdded));
        }

        [Test]
        public void AddBook_WhenCalledWithInActiveAuthor_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Build<AuthorArchived>()
                .FromFactory(() => new AuthorArchived(_author.Guid, _author.Books.Select(s => s.BookGuid).ToList(),
                    _author.AuthorQuotes.Select(s => s.QuoteGuid)))
                .Create();

            _author.ApplyChange(@event);
            _author.CommitEvents();

            Action act = () => _author.AddBook(new AuthorBook(_fixture.Create<Guid>()));
            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void
            AddBook_WhenCalledWithBookThatIsInAuthorsCollection_ShouldThrowBusinessRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();

            _author.AddBook(new AuthorBook(bookGuid));
            _author.CommitEvents();

            Action act = () => _author.AddBook(new AuthorBook(bookGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author cannot contain duplicated books.");
        }

        [Test]
        public void RemoveBook_WhenCalled_ShouldRemoveAuthorFromCollection()
        {
            var bookGuid = _fixture.Create<Guid>();

            _author.AddBook(new AuthorBook(bookGuid));
            _author.CommitEvents();

            var authorBook = _author.GetBook(bookGuid);

            _author.RemoveBook(authorBook);

            var @events = _author.GetUncommittedEvents();

            _author.Books.Should().HaveCount(0);

            @events.Should().HaveCount(1);
            @events.Should().Contain(p => p.GetType() == typeof(AuthorBookRemoved));
        }

        [Test]
        public void RemoveBook_WhenCalledWithInActiveAuthor_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Build<AuthorArchived>()
                .FromFactory(() => new AuthorArchived(_author.Guid, _author.Books.Select(s => s.BookGuid).ToList(),
                    _author.AuthorQuotes.Select(s => s.QuoteGuid)))
                .Create();

            _author.ApplyChange(@event);
            _author.CommitEvents();

            Action act = () => _author.RemoveBook(new AuthorBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveBook_WhenCalledAndBookIsNotPartOfAuthorsCollection_ShouldThrowBusinessRuleNotMeetException()
        {
            var bookGuid = _fixture.Create<Guid>();

            _author.AddBook(new AuthorBook(bookGuid));

            Action act = () => _author.RemoveBook(new AuthorBook(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author must have book in his collection.");
        }

        [Test]
        public void AddFollower_WhenCalled_ShouldAddNewFollower()
        {
            _author.AddFollower(new Follower(_followerGuid));

            var @events = _author.GetUncommittedEvents();

            _author.Followers.Should().HaveCount(1);
            _author.Followers.Should().Contain(p => p.FollowedBy == _followerGuid);

            @events.Should().HaveCount(1);
            @events.Should().Contain(p => p.GetType() == typeof(AuthorFollowed));
        }

        [Test]
        public void AddFollower_WhenCalledWithInActiveAuthor_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Build<AuthorArchived>()
                .FromFactory(() => new AuthorArchived(_author.Guid, _author.Books.Select(s => s.BookGuid).ToList(),
                    _author.AuthorQuotes.Select(s => s.QuoteGuid)))
                .Create();

            _author.ApplyChange(@event);
            _author.CommitEvents();

            Action act = () => _author.AddFollower(new Follower(_fixture.Create<Guid>()));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddFollower_WhenCalledWithFollowerThatAlreadyExists_ShouldThrowBussinesRuleNotMeetException()
        {
            _author.AddFollower(new Follower(_followerGuid));
            _author.CommitEvents();

            var follower = _author.GetFollower(_followerGuid);

            Action act = () => _author.AddFollower(follower);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author cannot have duplicated followers.");
        }

        [Test]
        public void RemoveFollower_WhenCalled_ShouldRemoveAuthorFollower()
        {
            _author.AddFollower(new Follower(_followerGuid));
            _author.CommitEvents();

            _author.RemoveFollower(new Follower(_followerGuid));

            var @events = _author.GetUncommittedEvents();

            _author.Followers.Should().HaveCount(0);

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(AuthorUnFollowed));
        }

        [Test]
        public void RemoveFollower_WhenCalledWithInActiveAuthor_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Build<AuthorArchived>()
                .FromFactory(() => new AuthorArchived(_author.Guid, _author.Books.Select(s => s.BookGuid).ToList(),
                    _author.AuthorQuotes.Select(s => s.QuoteGuid)))
                .Create();

            _author.ApplyChange(@event);
            _author.CommitEvents();

            Action act = () => _author.RemoveFollower(new Follower(_followerGuid));

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveFollower_AuthorHasNoFollower_ShouldThrowAuthorsFollowerMissing()
        {
            Action act = () => _author.RemoveFollower(new Follower(_followerGuid));

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Author does not have selected follower.");
        }

        [Test]
        public void AddGenre_WhenCalled_ShouldAddAuthorGenre()
        {
            _author.AddGenre(SubCategory.FictionSubCategory.Action);

            _author.Genres.Should().Contain(p => p == SubCategory.FictionSubCategory.Action);

            var @events = _author.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().Contain(p => p.GetType() == typeof(AuthorGenreAdded));
        }

        [Test]
        public void AddGenre_WhenCalledWithInActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Build<AuthorArchived>()
                .FromFactory(() => new AuthorArchived(_author.Guid, _author.Books.Select(s => s.BookGuid).ToList(),
                    _author.AuthorQuotes.Select(s => s.QuoteGuid)))
                .Create();

            _author.ApplyChange(@event);
            _author.CommitEvents();

            Action act = () => _author.AddGenre(SubCategory.FictionSubCategory.Romance);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddGenre_WhenCalledWithInvalidSubCategory_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => _author.AddGenre(Activator.CreateInstance(typeof(SubCategory), true) as SubCategory);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Invalid author genre.");
        }

        [Test]
        public void RemoveGenre_WhenCalled_ShouldRemoveAuthorGenre()
        {
            _author.AddGenre(SubCategory.FictionSubCategory.Action);
            _author.CommitEvents();

            _author.RemoveGenre(SubCategory.FictionSubCategory.Action);

            _author.Genres.Should().HaveCount(0);

            var @events = _author.GetUncommittedEvents();

            @events.Should().HaveCount(1);
            @events.Should().ContainSingle(p => p.GetType() == typeof(AuthorGenreRemoved));
        }

        [Test]
        public void RemoveGenre_WhenCalledWithInActiveAuthor_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = _fixture.Build<AuthorArchived>()
                .FromFactory(() => new AuthorArchived(_author.Guid, _author.Books.Select(s => s.BookGuid).ToList(),
                    _author.AuthorQuotes.Select(s => s.QuoteGuid)))
                .Create();

            _author.ApplyChange(@event);
            _author.CommitEvents();

            Action act = () => _author.RemoveGenre(SubCategory.FictionSubCategory.Action);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void RemoveGenre_WhenCalledWithInvalidGenre_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => _author.RemoveGenre(Activator.CreateInstance(typeof(SubCategory), true) as SubCategory);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Invalid author genre.");
        }

        [Test]
        public void GetFollower_WhenCalled_ShouldReturnAuthorFollower()
        {
            var addedFollower = new Follower(_followerGuid);
            _author.AddFollower(addedFollower);

            var follower = _author.GetFollower(_followerGuid);

            follower.Should().BeEquivalentTo(addedFollower);
        }

        [Test]
        public void GetFollower_WhenCalledWithMissingFollower_ShouldReturnNull()
        {
            var follower = _author.GetFollower(_fixture.Create<Guid>());

            follower.Should().BeNull();
        }

        [Test]
        public void GetBook_WhenCalled_ShouldReturnAuthorBook()
        {
            var addedBook = new AuthorBook(_fixture.Create<Guid>());
            _author.AddBook(addedBook);

            var book = _author.GetBook(addedBook.BookGuid);

            book.Should().NotBeNull();
            book.Should().BeEquivalentTo(addedBook);
        }

        [Test]
        public void GetBook_WhenCalledWithMissingBook_ShouldReturnNull()
        {
            var book = _author.GetBook(_fixture.Create<Guid>());

            book.Should().BeNull();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _followerGuid = _fixture.Create<Guid>();

            _author = (Author) Activator.CreateInstance(typeof(Author), true);

            var @event = _fixture.Create<AuthorCreated>()
                .WithAggregate(_fixture.Create<Guid>())
                .WithFullName(_fixture.Create<string>(), _fixture.Create<string>())
                .WithAddedBy(_fixture.Create<Guid>())
                .WithSex(Sex.Male.Value);

            _author.ApplyChange(@event);
            _author.CommitEvents();
        }
    }
}