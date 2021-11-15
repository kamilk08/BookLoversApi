using System;
using System.Linq;
using BaseTests.Aggregates.CommonRoot;
using BaseTests.Aggregates.EventSourcedAggregate;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.EventStore
{
    [TestFixture]
    public class SnapshotMakerTests
    {
        private ISnapshotMaker _snapshotMaker;
        private TestEventSourcedAggregateRoot _aggregateRoot;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshot()
        {
            Enumerable.Range(0, 11).ForEach(i => _aggregateRoot.DoWork());

            var snapshot = _snapshotMaker.MakeSnapshot(_aggregateRoot);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_aggregateRoot.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void MakeSnapshot_WhenCalledAndRootDoesNotHaveEnoughActionsToMakeSnapshot_ShouldNotMakeSnapshot()
        {
            Enumerable.Range(0, 9).ForEach(i => _aggregateRoot.DoWork());

            var snapshot = _snapshotMaker.MakeSnapshot(_aggregateRoot);

            snapshot.Should().BeNull();
        }

        [Test]
        public void MakeSnapshot_WhenCalledAndAggregateTypeIsInvalid_ShouldThrowInvalidCastException()
        {
            var aggregate = new DummyAggregateRoot();

            Action act = () => _snapshotMaker.MakeSnapshot(aggregate);

            act.Should().Throw<InvalidCastException>().WithMessage(
                $"Invalid type of aggregate. Aggregate of type ${aggregate.GetType().Name} cannot be snapshotted.");
        }

        [SetUp]
        public void SetUp()
        {
            _snapshotMaker = new SnapshotMaker();
            _aggregateRoot = new TestEventSourcedAggregateRoot();
        }
    }
}