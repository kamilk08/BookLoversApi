using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Domain.PublisherCycles;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Store.Persistence;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Mementos
{
    public class PublisherCycleMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private PublisherCycle _publisherCycle;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
                _publisherCycle.AddBook(new CycleBook(_fixture.Create<Guid>()));

            var snapshot = _snapshotMaker.MakeSnapshot(_publisherCycle);
            snapshot.AggregateGuid.Should().Be(_publisherCycle.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
                _publisherCycle.AddBook(new CycleBook(_fixture.Create<Guid>()));

            var snapshot = _snapshotMaker.MakeSnapshot(_publisherCycle);
            var publisherCycle = (PublisherCycle) ReflectionHelper.CreateInstance(typeof(PublisherCycle));

            var memento = _mementoFactoryMock.Object.Create<PublisherCycle>();
            memento = (IMemento<PublisherCycle>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());
            publisherCycle.ApplySnapshot(memento);
            publisherCycle.Should().BeEquivalentTo(_publisherCycle);
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<PublisherCycle>())
                .Returns(new PublisherCycleMemento());

            _fixture = new Fixture();
            _publisherCycle = new PublisherCycle(_fixture.Create<Guid>(), _fixture.Create<Guid>(),
                _fixture.Create<string>());

            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);
        }
    }
}