using System;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using FluentAssertions;
using NUnit.Framework;

namespace BaseTests.Aggregates.CommonRoot
{
    public class AggregateRootTests : AggregateRootSpecification<DummyAggregateRoot>
    {
        [Test]
        public void ActivateAggregate_WhenCalledAndAggregateIsAlreadyActive_ShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => this.AggregateRoot.ActivateAggregate();

            act.Should().Throw<BusinessRuleNotMetException>();
        }

        [Test]
        public void ArchiveAggregate_WhenCalled_ShouldArchiveAggregate()
        {
            AggregateRoot.ArchiveAggregate();

            AggregateRoot.Status.Should().Be(AggregateStatus.Archived.Value);
        }

        [Test]
        public void ArchiveAggregate_WhenCalledAndAggregateIsAlreadyArchived_ShouldThrowBusinessRuleNotMeetException()
        {
            this.AggregateRoot.ArchiveAggregate();

            Action act = () => this.AggregateRoot.ArchiveAggregate();

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Aggregate already archived");
        }

        [Test]
        public void DoWorkThatShouldFail_WhenCalled_AggregateShouldThrowBusinessRuleNotMeetException()
        {
            Action act = () => this.AggregateRoot.DoWorkThatShouldFail();

            act.Should().Throw<BusinessRuleNotMetException>().WithMessage("Broken rule message");
        }

        [Test]
        public void DoWorkThatProducesEvent_WhenCalled_AggregateShouldProduceSuccessResult()
        {
            AggregateRoot.DoWorkThatProducesEvent();

            Events.Should().HaveCount(1);
        }

        [Test]
        public void DoWorkThatShouldSucceed_WhenCalled_ShouldDoWork()
        {
            Action act = () => this.AggregateRoot.DoWorkThatShouldSucceed();

            act.Should().NotThrow<BusinessRuleNotMetException>();
        }

        protected override DummyAggregateRoot ConfigureAggregate()
        {
            return new DummyAggregateRoot();
        }
    }
}