using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.Services;
using BookLovers.Publication.Domain.Authors.Services.Editors;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.CoverTypes;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Events.Book;
using BookLovers.Shared;
using BookLovers.Shared.Categories;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Services
{
    [TestFixture]
    public class AuthorEditorTests
    {
        private AuthorEditService _editService;
        private Author _author;
        private AuthorData _authorData;
        private Fixture _fixture;
        private List<Book> _listOfBooks;
        private Mock<IUnitOfWork> _unitOfWorkMock;

        [Test]
        public void EditAuthor_WhenCalled_ShouldEditBook()
        {
            Action act = () => _editService.EditAuthor(_author, _authorData);
            act();
            act.Should().NotThrow<BusinessRuleNotMetException>();

            _author.Should().NotBeNull();
            _author.Basics.Sex.Should().Be(Sex.Female);
            _author.Basics.FullName.Should().NotBeNull();
            _author.Followers.Should().HaveCount(0);
            _author.Books.Should().HaveCount(_listOfBooks.Count);
            _author.Description.AboutAuthor.Should().Be(_authorData.Description.AboutAuthor);
            _author.Description.AuthorWebsite.Should().Be(_authorData.Description.WebSite);
            _author.Description.DescriptionSource.Should().Be(_authorData.Description.DescriptionSource);

            _author.Details.BirthDate.Should().Be(_authorData.Details.LifeLength.BirthDate);
            _author.Details.DeathDate.Should().Be(_authorData.Details.LifeLength.DeathDate);
            _author.Details.BirthPlace.Should().Be(_authorData.Details.BirthPlace);

            _author.Genres.Should().HaveCount(_authorData.Genres.Count);
            _author.Guid.Should().Be(_author.Guid);
        }

        [Test]
        public void EditAuthor_WhenCalledWithDifferentBasics_ShouldChangeOnlyBasics()
        {
            this._authorData
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()), Sex.Female);

            Action act = () => _editService.EditAuthor(_author, _authorData);
            act();
            act.Should().NotThrow<BusinessRuleNotMetException>();

            _author.Should().NotBeNull();
            _author.Basics.Sex.Should().Be(Sex.Female);

            var @events = _author.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<AuthorBasicsChanged>();
        }

        [Test]
        public void EditAuthor_WhenCalledWithDifferentDescription_ShouldChangeOnlyDescription()
        {
            this._authorData.WithDescription(_fixture.Create<string>(), _fixture.Create<string>(),
                _fixture.Create<string>());

            _editService.EditAuthor(this._author, this._authorData);

            _author.Should().NotBeNull();

            _author.Description.AboutAuthor.Should().Be(_authorData.Description.AboutAuthor);
            _author.Description.AuthorWebsite.Should().Be(_authorData.Description.WebSite);
            _author.Description.DescriptionSource.Should().Be(_authorData.Description.DescriptionSource);

            var @events = _author.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<AuthorDescriptionChanged>();
        }

        [Test]
        public void EditAuthor_WhenCalledWithDifferentDetails_ShouldChangeOnlyAuthorDetails()
        {
            this._authorData
                .WithDetails(
                    new LifeLength(_fixture.Create<DateTime>(), _fixture.Create<DateTime>()),
                    _fixture.Create<string>());

            _editService.EditAuthor(this._author, _authorData);

            _author.Should().NotBeNull();

            _author.Details.BirthDate.Should().Be(_authorData.Details.LifeLength.BirthDate);
            _author.Details.DeathDate.Should().Be(_authorData.Details.LifeLength.DeathDate);
            _author.Details.BirthPlace.Should().Be(_authorData.Details.BirthPlace);

            var @events = _author.GetUncommittedEvents();
            @events.Should().HaveCount(1);
            @events.Should().AllBeOfType<AuthorDetailsChanged>();
        }

        [Test]
        public void EditAuthor_WhenCalledWithDifferentGenres_ShouldChangeOnlyGenres()
        {
            _author.AddGenre(SubCategory.FictionSubCategory.Fantasy);
            _author.CommitEvents();

            this._authorData
                .WithGenres(new List<int>
                    { SubCategory.FictionSubCategory.Action.Value, SubCategory.NonFictionSubCategory.Health.Value });

            _editService.EditAuthor(this._author, this._authorData);

            _author.Should().NotBeNull();

            _author.Genres.Should().HaveCount(_authorData.Genres.Count);

            var @events = _author.GetUncommittedEvents();
            @events.Where(s => s.GetType() == typeof(AuthorGenreRemoved)).Should().HaveCount(2);
            @events.Where(s => s.GetType() == typeof(AuthorGenreAdded)).Should()
                .HaveCount(_authorData.Genres.Count);
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            var @event = CreateBookEvent();

            var book = ReflectionHelper.CreateInstance(typeof(Book)) as Book;

            book.ApplyChange(@event);

            _listOfBooks = new List<Book> { book };

            _unitOfWorkMock.Setup(s => s.GetAsync<Book>(book.Guid))
                .ReturnsAsync(book);

            var authorGuid = _fixture.Create<Guid>();
            var firstName = _fixture.Create<string>();
            var secondName = _fixture.Create<string>();

            _author = new Author(authorGuid, new FullName(firstName, secondName), Sex.Male);
            _author.AddBook(new AuthorBook(_listOfBooks.First().Guid));
            _author.CommitEvents();

            var authorEditors = new List<IAuthorEditor>();
            authorEditors.Add(new AuthorBasicsEditor());
            authorEditors.Add(new AuthorDescriptionEditor());
            authorEditors.Add(new AuthorDetailsEditor());
            authorEditors.Add(new AuthorGenresEditor());

            this._editService = new AuthorEditService(authorEditors);

            this._authorData = AuthorData.Initialize()
                .WithBasics(
                    new FullName(_fixture.Create<string>(), _fixture.Create<string>()),
                    Sex.Female)
                .WithDetails(
                    new LifeLength(
                        _fixture.Create<DateTime>(),
                        _fixture.Create<DateTime>()),
                    _fixture.Create<string>())
                .WithDescription(_fixture.Create<string>(), _fixture.Create<string>(), _fixture.Create<string>())
                .WithGuid(_author.Guid)
                .WithBooks(_listOfBooks.Select(s => s.Guid).ToList())
                .WithGenres(new List<int> { SubCategory.FictionSubCategory.Drama.Value });

            this._editService.EditAuthor(_author, _authorData);

            _author.CommitEvents();
        }

        private BookCreated CreateBookEvent()
        {
            var @event = _fixture.Create<BookCreated>()
                .WithAggregate(_fixture.Create<Guid>())
                .WithAuthors(_fixture.Create<List<Guid>>())
                .WithTitleAndIsbn(_fixture.Create<string>(), Isbn13Number())
                .WithPublisher(_fixture.Create<Guid>())
                .WithAddedBy(_fixture.Create<Guid>())
                .WithCategory(
                    Category.Fiction.Value,
                    Category.Fiction.Name)
                .WithSubCategory(
                    SubCategory.FictionSubCategory.Fantasy.Value,
                    SubCategory.FictionSubCategory.Fantasy.Name)
                .WithCover(_fixture.Create<string>(), CoverType.PaperBack.Value,
                    CoverType.PaperBack.Name)
                .WithHashTags(_fixture.Create<List<string>>());
            return @event;
        }

        private string Isbn13Number()
        {
            return "9788381168724";
        }
    }
}