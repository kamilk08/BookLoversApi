using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Bookcases.Domain;
using BookLovers.Bookcases.Domain.BookcaseBooks;
using BookLovers.Bookcases.Domain.Services;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Infrastructure.Mementos;
using BookLovers.Bookcases.Store.Persistence;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.UnitTests
{
    [TestFixture]
    public class BookcaseMementoTests
    {
        private Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private ISnapshotMaker _snapshotMaker;
        private Bookcase _bookcase;
        private Fixture _fixture;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_bookcase);

            var bookcase = (Bookcase) ReflectionHelper.CreateInstance(typeof(Bookcase));

            var bookcaseMemento = _mementoFactoryMock.Object.Create<Bookcase>();
            bookcaseMemento = (IMemento<Bookcase>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                bookcaseMemento.GetType(), SerializerSettings.GetSerializerSettings());

            bookcase.ApplySnapshot(bookcaseMemento);
            bookcase.Should().NotBe(null);
            bookcase.Should().BeEquivalentTo(_bookcase);
            bookcase.Shelves.Should().Equal(_bookcase.Shelves);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_bookcase);

            snapshot.AggregateGuid.Should().Be(_bookcase.Guid);

            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();

            _mementoFactoryMock.Setup(s => s.Create<Bookcase>())
                .Returns(new BookcaseMemento());

            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            _bookcase = new BookcaseFactory().Create(_fixture.Create<Guid>(), _fixture.Create<Guid>(),
                _fixture.Create<int>());

            var service = new BookcaseService();

            service.SetBookcaseWithSettings(
                _bookcase,
                new SettingsManager(_fixture.Create<Guid>(), _bookcase.Guid));

            PrepareBookcase(service);
        }

        private void PrepareBookcase(BookcaseService service)
        {
            for (var index = 0; index < 10; ++index)
            {
                var shelfGuid = _fixture.Create<Guid>();
                _bookcase.AddCustomShelf(shelfGuid, _fixture.Create<string>());
                _bookcase.ChangeShelfName(shelfGuid, _fixture.Create<string>());
                var shelf = _bookcase.GetShelf(shelfGuid);
                service.AddBook(
                    new BookcaseBook(_fixture.Create<Guid>(), _fixture.Create<Guid>(),
                        _fixture.Create<int>()), shelf);
            }
        }
    }
}