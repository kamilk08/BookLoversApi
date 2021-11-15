using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Events.Shelf;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.UnitTests
{
    [TestFixture]
    public class ShelfRecordTrackerTest
    {
        private Bookcase _bookcase;
        private Fixture _fixture;
        private BookcaseFactory _bookcaseFactory;
        private SettingsManager _settingsManager;
        private BookcaseService _bookcaseService;

        [Test]
        public void AddBook_WhenCalled_ShouldAddBookToBookcase()
        {
            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).Single();
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());

            _bookcaseService.AddBook(book, shelf);

            var @events = _bookcase.GetUncommittedEvents();

            _bookcase.Shelves.SelectMany(sm => sm.GetPublications()).ToList().Should()
                .Contain(book.BookGuid);

            @events.Count().Should().Be(1);
            @events.First().Should().BeOfType<BookAddedToShelf>();
        }

        [Test]
        public void AddBook_WhenCalledWithNotActiveBookcase_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = BookcaseArchived.Initialize()
                .WithBookcase(_bookcase.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid)
                .WithSettingsManager(_bookcase.Additions.SettingsManagerGuid)
                .WithShelfTracker(_bookcase.Additions.ShelfRecordTrackerGuid);

            _bookcase.ApplyChange(@event);

            var shelf = _bookcase.Shelves.First();
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());
            Action act = () => _bookcaseService.AddBook(book, shelf);

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddBook_WhenCalledMultipleTimesWithSameBook_ShouldThrowBusinessRuleNotMeetException()
        {
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();
            var shelfToAddEvent = _fixture.Create<CustomShelfCreated>()
                .WithAggregate(_bookcase.Guid)
                .WithShelf(shelfGuid, shelfName, ShelfCategory.Custom.Value);

            _bookcase.ApplyChange(shelfToAddEvent);
            _bookcase.CommitEvents();

            var shelf = _bookcase.GetShelf(shelfGuid);
            _bookcaseService.AddBook(book, shelf);

            Action act = () => _bookcaseService.AddBook(book, shelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Shelf already contains selected book.");
        }

        [Test]
        public void AddBook_WhenCalledAndShelfMaximumCapacityExceeded_ThenBusinessRuleNotMeetException()
        {
            _bookcase.AddCustomShelf(Shelf.CreateCustomShelf(Guid.NewGuid(), _fixture.Create<string>()));
            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.Custom).Single();
            for (var i = 0; i < ShelfCapacity.DefaultCapacity().SelectedOption + 1; i++)
                _bookcaseService.AddBook(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()), shelf);

            Action act = () =>
                _bookcaseService.AddBook(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()), shelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Shelf either does not exist or there is not enough enough space.");
        }

        [Test]
        public void ChangeShelf_WhenCalled_ShouldChangeBooksShelf()
        {
            var oldShelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).Single();
            var newShelf = _bookcase.GetShelvesByCategory(ShelfCategory.WantToRead).Single();

            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(),
                _fixture.Create<int>());

            var addedToShelfEvent = _fixture.Create<BookAddedToShelf>()
                .WithBookcase(_bookcase.Guid)
                .WithBookAndShelf(book.BookGuid, oldShelf.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid);

            _bookcase.ApplyChange(addedToShelfEvent);
            _bookcase.CommitEvents();

            _bookcaseService.ChangeShelf(book, oldShelf, newShelf);

            var @events = _bookcase.GetUncommittedEvents();

            _bookcase.GetShelvesByCategory(ShelfCategory.WantToRead).First().GetPublications()
                .Should().Contain(book.BookGuid);

            _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).First().GetPublications()
                .Should().NotContain(book.BookGuid);

            @events.Count().Should().Be(1);
            @events.First().Should().BeOfType<BookShelfChanged>();
        }

        [Test]
        public void ChangeShelf_WhenCalledWithInActiveBookcase_ShouldThrowBusinessRuleNotMeetException()
        {
            var oldShelf = _bookcase.GetShelvesByCategory(ShelfCategory.Read).First();
            var newShelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).First();

            var @event = BookcaseArchived.Initialize()
                .WithBookcase(_bookcase.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid)
                .WithSettingsManager(_bookcase.Additions.SettingsManagerGuid)
                .WithShelfTracker(_bookcase.Additions.ShelfRecordTrackerGuid);

            _bookcase.ApplyChange(@event);
            _bookcase.CommitEvents();

            Action act = () =>
                _bookcaseService.ChangeShelf(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()),
                    oldShelf, newShelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        public void ChangeShelf_WhenCalledWithBookThatIsNotOnSelectedShelf_ShouldThrowBusinessRuleNotMeetException()
        {
            var oldShelf = _bookcase.GetShelvesByCategory(ShelfCategory.Read).First();
            var newShelf = _bookcase.GetShelvesByCategory(ShelfCategory.WantToRead).First();

            Action act = () =>
                _bookcaseService.ChangeShelf(
                    new BookcaseBook(
                        _fixture.Create<Guid>(),
                        _fixture.Create<Guid>(),
                        _fixture.Create<int>()),
                    oldShelf, newShelf);

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Bookcase does not have selected book.");
        }

        [Test]
        public void ChangeShelf_WhenCalledWithOldShelfThatNotExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfName = _fixture.Create<string>();
            var shelfGuid = _fixture.Create<Guid>();

            var oldShelf = _fixture.Build<Shelf>()
                .FromFactory(() => Shelf.CreateCustomShelf(shelfGuid, shelfName))
                .Create();

            var newShelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).First();

            Action act = () =>
                _bookcaseService.ChangeShelf(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()),
                    oldShelf, newShelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected shelf.");
        }

        [Test]
        public void ChangeShelf_WhenCalledWithNewShelfThatNotExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfName = _fixture.Create<string>();
            var shelfGuid = _fixture.Create<Guid>();

            var newShelf = _fixture.Build<Shelf>()
                .FromFactory(() => Shelf.CreateCustomShelf(shelfGuid, shelfName))
                .Create();

            var oldShelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).First();

            Action act = () =>
                _bookcaseService.ChangeShelf(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()),
                    oldShelf, newShelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected shelf.");
        }

        [Test]
        public void ChangeShelf_ShelfMaximumCapacityExceeded_ShouldThrowShelfCapacityExceeded()
        {
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());
            _bookcase.AddCustomShelf(Shelf.CreateCustomShelf(Guid.NewGuid(), _fixture.Create<string>()));
            var customShelf = _bookcase.GetShelvesByCategory(ShelfCategory.Custom).Single();
            var wantToReadShelf = _bookcase.GetShelvesByCategory(ShelfCategory.WantToRead).Single();

            for (var i = 0; i < ShelfCapacity.DefaultCapacity().SelectedOption + 1; i++)
                _bookcaseService.AddBook(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()),
                    customShelf);

            _bookcaseService.AddBook(book, wantToReadShelf);
            var oldShelf = _bookcase.GetShelvesByCategory(ShelfCategory.WantToRead).First();

            Action act = () => _bookcaseService.ChangeShelf(book, oldShelf, customShelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Shelf either does not exist or there is not enough enough space.");
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _bookcaseFactory = new BookcaseFactory();
            _bookcaseService = new BookcaseService();

            var bookcaseGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();
            var readerId = _fixture.Create<int>();

            _bookcase = _bookcaseFactory.Create(bookcaseGuid, readerGuid, readerId);
            _bookcase.CommitEvents();

            _settingsManager = new SettingsManager(_bookcase.Additions.SettingsManagerGuid, _bookcase.Guid);
            _bookcaseService.SetBookcaseWithSettings(_bookcase, _settingsManager);
        }
    }
}