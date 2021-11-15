using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Domain.SeriesCycle;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Store.Persistence;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Mementos
{
    public class SeriesMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Series _series;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            for (var index = 1; index < 20; ++index)
                _series.AddToSeries(new SeriesBook(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            var series = _snapshotMaker.MakeSnapshot(_series);

            series.Should().NotBeNull();
            series.AggregateGuid.Should().Be(_series.Guid);
            series.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 20; ++index)
                _series.AddToSeries(new SeriesBook(_fixture.Create<Guid>(), _fixture.Create<byte>()));

            var snapshot = _snapshotMaker.MakeSnapshot(_series);
            var series = (Series) ReflectionHelper.CreateInstance(typeof(Series));

            var memento = _mementoFactoryMock.Object.Create<Series>();

            memento = (IMemento<Series>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            series.ApplySnapshot(memento);
            series.Should().BeEquivalentTo(_series);
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Series>())
                .Returns(new SeriesMemento());

            _fixture = new Fixture();
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);
            _series = new Series(_fixture.Create<Guid>(), _fixture.Create<string>());
        }
    }
}