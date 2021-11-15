using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Events;
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
    public class BookcaseTests
    {
        private Bookcase _bookcase;
        private Fixture _fixture;
        private BookcaseService _bookcaseService;

        [Test]
        [TestCase("CUSTOM_SHELF_NAME")]
        [TestCase("ABC")]
        [TestCase("123")]
        [TestCase("A1B2C3")]
        public void AddCustomShelf_WhenCalled_ShouldAddNewCustomShelfToBookCase(string shelfName)
        {
            _bookcase.AddCustomShelf(Guid.NewGuid(), shelfName);

            var shelf = _bookcase.GetShelf(shelfName);

            var @events = _bookcase.GetUncommittedEvents();

            @events.Count().Should().Be(1);
            @events.First().Should().BeOfType<CustomShelfCreated>();

            _bookcase.Shelves.Where(s => s.ShelfDetails.Category == ShelfCategory.Custom)
                .Select(s => s.ShelfDetails.ShelfName).Should().NotBeEmpty();
            _bookcase.Shelves.Where(s => s.ShelfDetails.Category == ShelfCategory.Custom)
                .Select(s => s.ShelfDetails.ShelfName).Should().BeEquivalentTo(shelfName);
            _bookcase.Shelves.Where(p => p.ShelfDetails.ShelfName == shelfName)
                .Select(s => s.ShelfDetails.Category).Should().Equal(ShelfCategory.Custom);
        }

        [Test]
        public void AddCustomShelf_WhenCalledButBookcaseHasBeenArchived_ShouldThrowBusinessRuleNotMeetException()
        {
            var @event = BookcaseArchived.Initialize()
                .WithBookcase(_bookcase.Guid)
                .WithReader(Guid.NewGuid())
                .WithSettingsManager(_bookcase.Additions.SettingsManagerGuid)
                .WithShelfTracker(_bookcase.Additions.ShelfRecordTrackerGuid);

            _bookcase.ApplyChange(@event);

            Action act = () => _bookcase.AddCustomShelf(Guid.NewGuid(), "SHELF_NAME");

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void AddCustomShelf_WhenCalledWithShelfThatAlreadyExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfGuid = Guid.NewGuid();
            var shelfName = "TEST_SHELF";
            var category = ShelfCategory.Custom.Value;
            _bookcase.ApplyChange(new CustomShelfCreated(_bookcase.Guid, shelfGuid, shelfName, category));

            Action act = () => _bookcase.AddCustomShelf(shelfGuid, shelfName);

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Bookcase cannot have duplicated shelves.");
        }

        [Test]
        public void AddCustomShelf_TriesToAddSameShelfToBookCase_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();

            _bookcase.AddCustomShelf(shelfGuid, shelfName);

            Action act = () => _bookcase.AddCustomShelf(shelfGuid, shelfName);

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Bookcase cannot have duplicated shelves.");
        }

        [Test]
        public void RemoveShelf_WhenCalled_ShouldRemoveCustomShelfFromBookCase()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();
            var @event = _fixture.Create<CustomShelfCreated>()
                .WithAggregate(_bookcase.Guid)
                .WithShelf(shelfGuid, shelfName, ShelfCategory.Custom.Value);

            _bookcase.ApplyChange(@event);
            _bookcase.CommitEvents();
            var shelfsCount = _bookcase.Shelves.Count;
            var shelfToRemove = _bookcase.GetShelf(shelfGuid);

            _bookcase.RemoveShelf(shelfToRemove);

            var @events = _bookcase.GetUncommittedEvents();

            _bookcase.Shelves.Count.Should().Be(shelfsCount - 1);
            _bookcase.Shelves.Should().BeOfType<List<Shelf>>();
            _bookcase.Shelves.Select(s => s.Guid).Should().NotBeEquivalentTo(new List<Guid>() { shelfGuid });

            @events.Count().Should().Be(1);
            @events.First().Should().BeOfType<ShelfRemoved>();
        }

        [Test]
        public void RemoveShelf_WhenCalledWithInactive_ShouldThrowBookCaseNotActiveException()
        {
            var @event = BookcaseArchived.Initialize()
                .WithBookcase(_bookcase.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid)
                .WithSettingsManager(_bookcase.Additions.SettingsManagerGuid)
                .WithShelfTracker(_bookcase.Additions.ShelfRecordTrackerGuid);

            _bookcase.ApplyChange(@event);
            var shelf = _bookcase.Shelves.First();

            Action act = () => _bookcase.RemoveShelf(shelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        public void RemoveShelf_ThereIsNoShelfWithGivenIdInBookCase_ShouldThrowShelfMissingException()
        {
            var customShelfName = _fixture.Create<string>();
            var customShelf = _fixture.Build<Shelf>()
                .FromFactory(() => Shelf.CreateCustomShelf(Guid.NewGuid(), customShelfName))
                .Create();

            Action act = () => _bookcase.RemoveShelf(customShelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected shelf.");
        }

        [Test]
        public void RemoveShelf_WhenCalledWithShelfThatIsNotOfTypeCustom_ShouldThrowBusinessRuleNotMeetException()
        {
            var nowReadingShelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).Single();

            Action act = () => _bookcase.RemoveShelf(nowReadingShelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Shelf type is not custom.");
        }

        [Test]
        public void RemoveShelf_WhenCalledWithShelfThatDoesNotExists_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();

            var shelf = _fixture.Build<Shelf>().FromFactory(() => Shelf.CreateCustomShelf(shelfGuid, shelfName))
                .Create();

            Action act = () => _bookcase.RemoveShelf(shelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected shelf.");
        }

        [Test]
        public void RemoveFromShelf_WhenCalled_ShouldRemoveBookFromBookcase()
        {
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(),
                _fixture.Create<int>());
            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).Single();

            var @event = _fixture.Create<BookAddedToShelf>().WithBookcase(_bookcase.Guid)
                .WithBookAndShelf(book.BookGuid, shelf.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid);

            _bookcase.ApplyChange(@event);
            _bookcase.CommitEvents();

            _bookcase.RemoveFromShelf(book, shelf);

            var @events = _bookcase.GetUncommittedEvents();

            @events.Count().Should().Be(1);
            @events.First().Should().BeOfType<BookRemovedFromShelf>();

            _bookcase.Shelves.SelectMany(sm => sm.GetPublications()).Count().Should().Be(0);
        }

        [Test]
        public void RemoveFromShelf_WhenCalledAndBookcaseIsNotActive_ShouldThrowBusinessRuleNotMeetException()
        {
            var book = new BookcaseBook(
                _fixture.Create<Guid>(),
                _fixture.Create<Guid>(),
                _fixture.Create<int>());

            var @event = BookcaseArchived.Initialize()
                .WithBookcase(_bookcase.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid)
                .WithSettingsManager(_bookcase.Additions.SettingsManagerGuid)
                .WithShelfTracker(_bookcase.Additions.ShelfRecordTrackerGuid);

            _bookcase.ApplyChange(@event);

            Action act = () => _bookcase.RemoveFromShelf(book, null);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        public void
            RemoveFromShelf_WhenCalledAndThereIsNoSelectedBookInBookcase_ShouldThrowBussinesRuleNotMeetException()
        {
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());
            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.WantToRead).First();

            Action act = () => _bookcase.RemoveFromShelf(book, shelf);

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected book.");
        }

        [Test]
        public void Contains_WhenCalledAndGivenBookIsInBookCase_ShouldReturnTrue()
        {
            var book = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());
            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.NowReading).First();
            _bookcaseService.AddBook(book, shelf);

            for (var i = 0; i < ShelfCapacity.MinCapacity - 1; i++)
                _bookcaseService.AddBook(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>()), shelf);

            var result = _bookcase.ContainsBook(book.BookGuid);

            result.Should().BeTrue();
            _bookcase.Shelves.SelectMany(sm => sm.GetPublications()).Count()
                .Should().Be(ShelfCapacity.MinCapacity);
        }

        [Test]
        public void GetShelvesByCategory_WhenCalled_ShouldGetSelectedShelf()
        {
            for (var i = 0; i < 10; i++)
                _bookcase.AddCustomShelf(
                    _fixture.Create<Guid>(),
                    _fixture.Create<string>());

            var shelves = _bookcase.GetShelvesByCategory(ShelfCategory.Custom);

            shelves.Should().NotBeNull();
            shelves.Should().BeOfType<List<Shelf>>();
            shelves.Select(s => s.ShelfDetails.Category.Value).Should().AllBeEquivalentTo(ShelfCategory.Custom.Value);
        }

        [Test]
        [TestCase("Read")]
        [TestCase("Now reading")]
        [TestCase("Want to read")]
        [TestCase("CUSTOM_SHELF_NAME")]
        public void GetShelf_WhenCalledWithShelfName_ShouldGetSelectedShelf(string shelfName)
        {
            var shelfGuid = _fixture.Create<Guid>();

            _bookcase.AddCustomShelf(shelfGuid, "CUSTOM_SHELF_NAME");
            var shelf = _bookcase.GetShelf(shelfName);

            shelf.Should().NotBeNull();
            shelf.ShelfDetails.ShelfName.Should().Be(shelfName);
        }

        [Test]
        public void GetShelf_WhenCalledWithShelfNameThatDoesNotExists_ShouldReturnDefaultValue()
        {
            var shelfName = _fixture.Create<string>();

            var shelf = _bookcase.GetShelf(shelfName);

            shelf.Should().BeNull();
        }

        [Test]
        public void GetShelf_WhenCalledWithShelfGuid_ShouldReturnShelfThatExists()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();

            _bookcase.AddCustomShelf(shelfGuid, shelfName);

            var shelf = _bookcase.GetShelf(shelfGuid);

            shelf.Should().NotBeNull();
            shelf.Guid.Should().Be(shelfGuid);
        }

        [Test]
        public void ChangeShelfName_WhenCalled_ShouldChangeShelfName()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();

            _bookcase.AddCustomShelf(shelfGuid, shelfName);
            _bookcase.CommitEvents();

            _bookcase.ChangeShelfName(shelfGuid, "NEW SHELF NAME");

            var shelf = _bookcase.GetShelf(shelfGuid);

            var @events = _bookcase.GetUncommittedEvents();

            shelf.ShelfDetails.ShelfName.Should().Be("NEW SHELF NAME");
            @events.Count().Should().Be(1);
            @events.First().Should().BeOfType<ShelfNameChanged>();
        }

        [Test]
        public void ChangeShelfName_WhenCalledWithInActiveBookcase_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();

            _bookcase.AddCustomShelf(shelfGuid, shelfName);

            var @event = BookcaseArchived.Initialize()
                .WithBookcase(_bookcase.Guid)
                .WithReader(_bookcase.Additions.ReaderGuid)
                .WithSettingsManager(_bookcase.Additions.SettingsManagerGuid)
                .WithShelfTracker(_bookcase.Additions.ShelfRecordTrackerGuid);

            _bookcase.ApplyChange(@event);
            _bookcase.CommitEvents();

            Action act = () => _bookcase.ChangeShelfName(shelfGuid, _fixture.Create<string>());

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Operation that was invoked on aggregate cannot be performed. Aggregate is not active.");
        }

        [Test]
        public void ChangeShelfName_WhenCalledWithMissingShelf_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelfGuid = _fixture.Create<Guid>();

            Action act = () => _bookcase.ChangeShelfName(shelfGuid, _fixture.Create<string>());

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Bookcase does not have selected shelf.");
        }

        [Test]
        public void ChangeShelfName_WhenCalledWithShelfThatIsNotCustomType_ShouldThrowBusinessRuleNotMeetException()
        {
            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.Read).First();

            Action act = () => _bookcase.ChangeShelfName(shelf.Guid, _fixture.Create<string>());

            act.Should().Throw<BusinessRuleNotMetException>()
                .WithMessage("Shelf type is not custom.");
        }

        [Test]
        public void ContainsBook_WhenCalled_ShouldReturnBookThatIsInBookcase()
        {
            var book = new BookcaseBook(
                _fixture.Create<Guid>(),
                _fixture.Create<Guid>(),
                _fixture.Create<int>());

            var shelf = _bookcase.GetShelvesByCategory(ShelfCategory.Read).First();

            _bookcaseService.AddBook(book, shelf);
            _bookcase.CommitEvents();

            var contains = _bookcase.ContainsBook(book.BookGuid);

            contains.Should().BeTrue();
        }

        [Test]
        public void ContainsBook_WhenCalledWithBookThatIsNotInBookcase_ShouldReturnFalse()
        {
            var bookGuid = _fixture.Create<Guid>();

            var contains = _bookcase.ContainsBook(bookGuid);

            contains.Should().BeFalse();
        }

        [Test]
        public void GetShelvesByCategory_WhenCalled_ShouldReturnDesiredShelves()
        {
            var shelves = _bookcase.GetShelvesByCategory(ShelfCategory.Read);

            shelves.Select(s => s.ShelfDetails.Category).Should().AllBeEquivalentTo(ShelfCategory.Read);
        }

        [Test]
        public void GetShelvesByCategory_WhenCalledWhenThereIsNoCustomShelves_ShouldReturnEmptyList()
        {
            var shelfs = _bookcase.GetShelvesByCategory(ShelfCategory.Custom);

            shelfs.Count.Should().Be(0);
        }

        [Test]
        public void GetShelf_WhenCalledWithNameThatOneOfTheShelvesHave_ShouldReturnDesiredShelf()
        {
            var shelf = _bookcase.GetShelf("Read");

            shelf.Should().NotBeNull();
            shelf.ShelfDetails.Category.Should().BeEquivalentTo(ShelfCategory.Read);
            shelf.ShelfDetails.ShelfName.Should().Be("Read");
        }

        [Test]
        public void GetShelf_WhenCalledWithNameThatNoneOfTheShelfsHave_ShouldReturnNull()
        {
            var shelf = _bookcase.GetShelf(_fixture.Create<string>());

            shelf.Should().BeNull();
        }

        [Test]
        public void GetShelf_WhenCalledWithGuid_ShouldReturnShelf()
        {
            var shelfGuid = _fixture.Create<Guid>();
            var shelfName = _fixture.Create<string>();

            _bookcase.AddCustomShelf(shelfGuid, shelfName);

            var shelf = _bookcase.GetShelf(shelfGuid);

            shelf.Should().NotBeNull();
            shelf.Guid.Should().Be(shelfGuid);
            shelf.ShelfDetails.Category.Should().BeEquivalentTo(ShelfCategory.Custom);
        }

        [Test]
        public void GetShelvesWithBook_WhenCalled_ShouldReturnShelvesWithBook()
        {
            var customShelfGuid = _fixture.Create<Guid>();
            var firstBook = new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<int>());

            _bookcase.AddCustomShelf(customShelfGuid, _fixture.Create<string>());

            var readedShelf = _bookcase.GetShelvesByCategory(ShelfCategory.Read).First();
            var customShelf = _bookcase.GetShelf(customShelfGuid);

            _bookcaseService.AddBook(firstBook, readedShelf);
            _bookcaseService.AddBook(firstBook, customShelf);

            var shelvesWithBook = _bookcase.GetShelvesWithBook(firstBook.BookGuid);

            shelvesWithBook.Should().NotBeNull();
            shelvesWithBook.Count().Should().Be(2);
            shelvesWithBook.Contains(readedShelf).Should().BeTrue();
            shelvesWithBook.Contains(customShelf).Should().BeTrue();
        }

        [Test]
        public void GetShelvesWithBook_WhenCalledWithBookThatIsNotInBookcase_ShouldReturnEmptyList()
        {
            var shelvesWithBook = _bookcase.GetShelvesWithBook(_fixture.Create<Guid>());

            shelvesWithBook.Should().NotBeNull();
            shelvesWithBook.Count().Should().Be(0);
        }

        [Test]
        public void HasShelf_WhenCalledWithShelfGuid_ShouldReturnTrue()
        {
            var shelfGuid = _fixture.Create<Guid>();
            _bookcase.AddCustomShelf(shelfGuid, _fixture.Create<string>());

            var hasShelf = _bookcase.HasShelf(shelfGuid);

            hasShelf.Should().BeTrue();
        }

        [Test]
        public void HasShelf_WhenCalledWithShelfThatNotExists_ShouldReturnFalse()
        {
            var hasShelf = _bookcase.HasShelf(_fixture.Create<Guid>());

            hasShelf.Should().BeFalse();
        }

        [Test]
        public void HasShelf_WhenCalledWithShelfNameThatIsInBookcase_ShouldReturnTrue()
        {
            var hasShelf = _bookcase.HasShelf("Read");

            hasShelf.Should().BeTrue();
        }

        [Test]
        public void HasShelf_WhenCalledWithShelfNameThatIsNotInBookcase_ShouldReturnFalse()
        {
            var has = _bookcase.HasShelf(_fixture.Create<string>());

            has.Should().BeFalse();
        }

        [SetUp]
        public void SetUp()
        {
            var @eventsList = new List<IEvent>();
            _fixture = new Fixture();

            var bookcaseGuid = _fixture.Create<Guid>();
            var readerGuid = _fixture.Create<Guid>();

            _bookcase = (Bookcase) Activator.CreateInstance(typeof(Bookcase), true);

            IEvent @event = BookcaseCreated.Initialize()
                .WithBookcase(bookcaseGuid)
                .WithReader(readerGuid, _fixture.Create<int>())
                .WithSettingsManager(_fixture.Create<Guid>());

            @eventsList.Add(@event);

            @event = CoreShelfCreated.Initialize()
                .WithBookcase(bookcaseGuid)
                .WithShelf("Now reading", ShelfCategory.NowReading.Value);

            @eventsList.Add(@event);

            @event = CoreShelfCreated.Initialize()
                .WithBookcase(bookcaseGuid)
                .WithShelf("Want to read", ShelfCategory.WantToRead.Value);

            @eventsList.Add(@event);

            @event = CoreShelfCreated.Initialize().WithBookcase(bookcaseGuid)
                .WithShelf("Read", ShelfCategory.Read.Value);

            @eventsList.Add(@event);

            foreach (var @eve in eventsList)
                _bookcase.ApplyChange(@eve);

            _bookcase.CommitEvents();

            var settingsManager = new SettingsManager(_bookcase.Additions.SettingsManagerGuid, _bookcase.Guid);

            _bookcaseService = new BookcaseService();
            _bookcaseService.SetBookcaseWithSettings(_bookcase, settingsManager);
        }
    }
}