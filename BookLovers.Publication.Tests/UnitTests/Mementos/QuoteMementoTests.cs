using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Domain.Quotes;
using BookLovers.Publication.Events.Quotes;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared.Likes;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Mementos
{
    public class QuoteMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Quote _quote;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
                _quote.AddLike(Like.NewLike(_fixture.Create<Guid>()));

            var snapshot = _snapshotMaker.MakeSnapshot(_quote);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_quote.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
                _quote.AddLike(Like.NewLike(_fixture.Create<Guid>()));

            var snapshot = _snapshotMaker.MakeSnapshot(_quote);
            var quote = (Quote) ReflectionHelper.CreateInstance(typeof(Quote));

            var memento = _mementoFactoryMock.Object.Create<Quote>();
            memento = (IMemento<Quote>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            quote.ApplySnapshot(memento);
            quote.Should().BeEquivalentTo(_quote);
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Quote>())
                .Returns(new QuoteMemento());

            _fixture = new Fixture();
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            _quote = (Quote) ReflectionHelper.CreateInstance(typeof(Quote));
            _quote.ApplyChange(BookQuoteAdded.Initialize().WithAggregate(_fixture.Create<Guid>())
                .WithBook(_fixture.Create<Guid>()).WithQuote(_fixture.Create<string>(), _fixture.Create<DateTime>())
                .WithAddedBy(_fixture.Create<Guid>()));

            _quote.CommitEvents();
        }
    }
}