using System.Collections.Generic;
using System.Linq;
using BaseTests.Aggregates.DummyEvents;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Infrastructure.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.Aggregates.EventSourcedAggregate
{
    public class EventSourcedAggregateRootTests :
        AggregateRootSpecification<TestEventSourcedAggregateRoot>
    {
        [Test]
        public void DoWork_WhenCalled_ShouldCreateDesiredEvent()
        {
            AggregateRoot.DoWork();

            Events.Should().HaveCount(1);
            Events.Should().AllBeOfType<DoWorkEvent>();
            AggregateRoot.Counter.Should().Be(1);
        }

        [Test]
        public void DoWork_WhenCalledMultipleTimes_ShouldCreateMultipleEvents()
        {
            Enumerable.Range(0, 5).ForEach(i => AggregateRoot.DoWork());

            AggregateRoot.Counter.Should().Be(5);
            AggregateRoot.Version.Should().Be(Events.Count() - 1);
        }

        [Test]
        public void ClearWork_WhenCalled_ShouldCreateDesiredEvent()
        {
            AggregateRoot.ClearWork();

            Events.Should().HaveCount(1);
            Events.Should().AllBeOfType<ClearWorkEvent>();
        }

        [Test]
        public void ApplyChange_WhenCalled_ShouldApplyEventOnAggregate()
        {
            AggregateRoot.ApplyChange(new DoWorkEvent(AggregateRoot.Guid, 10));

            AggregateRoot.Counter.Should().Be(10);
        }

        [Test]
        public void IsActive_WhenCalled_ShouldReturnTrue() => AggregateRoot.IsActive().Should().BeTrue();

        [Test]
        public void ReHydrateAggregate_WhenCalled_ShouldRecreateAggregate()
        {
            AggregateRoot.RehydrateAggregate(new List<IEvent>
            {
                new DoWorkEvent(AggregateRoot.Guid, 1),
                new DoWorkEvent(AggregateRoot.Guid, 2),
                new DoWorkEvent(AggregateRoot.Guid, 3),
                new DoWorkEvent(AggregateRoot.Guid, 4)
            });

            AggregateRoot.Counter.Should().Be(4);
        }

        protected override TestEventSourcedAggregateRoot ConfigureAggregate()
        {
            return new TestEventSourcedAggregateRoot();
        }
    }
}