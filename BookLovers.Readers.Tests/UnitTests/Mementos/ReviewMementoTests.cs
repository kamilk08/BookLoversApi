using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Domain.Reviews;
using BookLovers.Readers.Infrastructure.Mementos;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.Likes;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Mementos
{
    public class ReviewMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Review _review;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_review);
            var review = (Review) ReflectionHelper.CreateInstance(typeof(Review));

            var memento = _mementoFactoryMock.Object.Create<Review>();

            memento = (IMemento<Review>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            review.ApplySnapshot(memento);
            review.Should().NotBeNull();
            review.Should().NotBe(null);
            review.Should().BeEquivalentTo(_review);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_review);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_review.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Review>())
                .Returns(new ReviewMemento());

            _fixture = new Fixture();
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);
            _review = new Review(
                _fixture.Create<Guid>(),
                new ReviewIdentification(_fixture.Create<Guid>(), _fixture.Create<Guid>()),
                new ReviewContent(_fixture.Create<string>(), _fixture.Create<DateTime>()),
                new ReviewSpoiler(_fixture.Create<bool>(), _fixture.Create<bool>()));
            PrepareReview();
        }

        private void PrepareReview()
        {
            for (var index = 0; index < 5; ++index)
            {
                _review.AddLike(Like.NewLike(_fixture.Create<Guid>()));
                _review.AddSpoilerTag(new SpoilerTag(_fixture.Create<Guid>()));
                _review.Report(new ReviewReport(_fixture.Create<Guid>()));
            }

            _review.EditReview(
                _review.ReviewIdentification.ReaderGuid,
                new ReviewContent(_fixture.Create<string>(), _fixture.Create<DateTime>()), true);
        }
    }
}