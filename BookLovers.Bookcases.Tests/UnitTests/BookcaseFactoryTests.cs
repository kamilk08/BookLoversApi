using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.ShelfCategories;
using BookLovers.Bookcases.Events.Bookcases;
using BookLovers.Bookcases.Events.Shelf;
using FluentAssertions;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.UnitTests
{
    [TestFixture]
    public class BookcaseFactoryTests
    {
        private Fixture _fixture;
        private BookcaseFactory _bookcaseFactory;
        private Guid _bookcaseGuid;
        private Guid _readerGuid;
        private int _readerId;
        private int _expectedVersion = 3;
        private int _expectedShelvesCount = 3;
        private int _expectedEventsCount = 4;

        [Test]
        public void Create_WhenCalled_ShouldCreateBookcase()
        {
            var bookcase = _bookcaseFactory.Create(_bookcaseGuid, _readerGuid, _readerId);

            var @events = bookcase.GetUncommittedEvents();

            bookcase.Guid.Should().Be(_bookcaseGuid);
            bookcase.Additions.ReaderGuid.Should().Be(_readerGuid);
            bookcase.Additions.SettingsManagerGuid.Should().NotBeEmpty();
            bookcase.Version.Should().Be(_expectedVersion);
            bookcase.Shelves.Count.Should().Be(_expectedShelvesCount);
            bookcase.Shelves.Select(s => s.ShelfDetails.Category)
                .Should().NotBeEquivalentTo(new List<ShelfCategory>() { ShelfCategory.Custom });
            bookcase.AggregateStatus.Should().Be(AggregateStatus.Active);

            @events.Count().Should().Be(_expectedEventsCount);
            @events.First().Should().BeOfType<BookcaseCreated>();
            @events.Skip(1).Should().AllBeOfType<CoreShelfCreated>();
        }

        [Test]
        public void Create_WhenCalledWithValidInputs_ShouldNotThrowBussinesRuleNotMeetException()
        {
            Action act = () => _bookcaseFactory.Create(_bookcaseGuid, _readerGuid, _readerId);

            act.Should().NotThrow<BusinessRuleNotMetException>();
        }

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();

            _bookcaseGuid = _fixture.Create<Guid>();
            _readerGuid = _fixture.Create<Guid>();
            _readerId = _fixture.Create<int>();

            _bookcaseFactory = new BookcaseFactory();
        }
    }
}