using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Domain.Publishers;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Store.Persistence;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Mementos
{
    public class PublisherMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Publisher _publisher;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
            {
                _publisher.AddBook(new PublisherBook(_fixture.Create<Guid>()));
                _publisher.AddCycle(new Cycle(_fixture.Create<Guid>()));
            }

            var snapshot = _snapshotMaker.MakeSnapshot(_publisher);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_publisher.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
            {
                _publisher.AddBook(new PublisherBook(_fixture.Create<Guid>()));
                _publisher.AddCycle(new Cycle(_fixture.Create<Guid>()));
            }

            var snapshot = _snapshotMaker.MakeSnapshot(_publisher);

            var publisher = (Publisher) ReflectionHelper.CreateInstance(typeof(Publisher));

            var memento = _mementoFactoryMock.Object.Create<Publisher>();
            memento = (IMemento<Publisher>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            publisher.ApplySnapshot(memento);
            publisher.Should().BeEquivalentTo(_publisher);
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Publisher>())
                .Returns(new PublisherMemento());

            _fixture = new Fixture();

            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            _publisher = new Publisher(_fixture.Create<Guid>(), _fixture.Create<string>());
        }
    }
}