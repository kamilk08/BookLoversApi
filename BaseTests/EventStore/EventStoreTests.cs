using System.Linq;
using System.Threading.Tasks;
using BaseTests.Aggregates.EventSourcedAggregate;
using BookLovers.Base.Infrastructure.Persistence;
using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.EventStore
{
    [TestFixture]
    public class EventStoreTests
    {
        private IEventStore _eventStore;
        private TestEventSourcedAggregateRoot _aggregate;

        [Test]
        public async Task GetAsync_WhenCalledAndAggregateIsMadeFromScratch_ShouldReturnAggregateFromEventStore()
        {
            var aggregateRoot = await _eventStore.GetAsync<TestEventSourcedAggregateRoot>(_aggregate.Guid);

            aggregateRoot.Guid.Should().Be(_aggregate.Guid);
            aggregateRoot.Counter.Should().Be(_aggregate.Counter);
            aggregateRoot.Version.Should().Be(_aggregate.Version);
            aggregateRoot.AggregateStatus.Should().Be(_aggregate.AggregateStatus);
        }

        [Test]
        public async Task GetAsync_WhenCalledAndAggregateIsMadeFromSnapshot_ShouldReturnAggregateFromEventStore()
        {
            Enumerable.Range(0, 13).ToList().ForEach((i) => _aggregate.DoWork());
            await _eventStore.StoreEvents(_aggregate);

            var aggregateRoot = await _eventStore.GetAsync<TestEventSourcedAggregateRoot>(_aggregate.Guid);

            aggregateRoot.Guid.Should().Be(_aggregate.Guid);
            aggregateRoot.Counter.Should().Be(_aggregate.Counter);
            aggregateRoot.Version.Should().Be(_aggregate.Version);
            aggregateRoot.AggregateStatus.Should().Be(_aggregate.AggregateStatus);
        }

        [SetUp]
        public async Task SetUp()
        {
            _eventStore = new InMemoryEventStore(
                new InMemoryEventStream(),
                new MementoFactory(),
                new SnapshotMaker(),
                new InMemorySnapshotProvider());

            _aggregate = new TestEventSourcedAggregateRoot();

            _aggregate.DoWork();
            _aggregate.DoWork();
            _aggregate.ClearWork();
            _aggregate.DoWork();

            await _eventStore.StoreEvents(_aggregate);

            _aggregate.CommitEvents();
        }
    }
}