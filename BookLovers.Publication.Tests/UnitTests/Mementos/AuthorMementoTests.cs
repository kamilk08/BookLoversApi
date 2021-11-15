using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Events.Authors;
using BookLovers.Publication.Infrastructure.Mementos;
using BookLovers.Publication.Store.Persistence;
using BookLovers.Shared;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Publication.Tests.UnitTests.Mementos
{
    public class AuthorMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Author _author;
        private SnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
            {
                _author.AddBook(new AuthorBook(_fixture.Create<Guid>()));
                _author.AddFollower(new Follower(_fixture.Create<Guid>()));
                _author.AddQuote(new AuthorQuote(_fixture.Create<Guid>()));
            }

            var snapshot = _snapshotMaker.MakeSnapshot(_author);

            snapshot.Should().NotBeNull();
            snapshot.AggregateGuid.Should().Be(_author.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            for (var index = 0; index < 10; ++index)
            {
                _author.AddBook(new AuthorBook(_fixture.Create<Guid>()));
                _author.AddFollower(new Follower(_fixture.Create<Guid>()));
                _author.AddQuote(new AuthorQuote(_fixture.Create<Guid>()));
            }

            var snapshot = _snapshotMaker.MakeSnapshot(_author);
            var author = (Author) ReflectionHelper.CreateInstance(typeof(Author));
            var memento = _mementoFactoryMock.Object.Create<Author>();

            memento = (IMemento<Author>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            author.ApplySnapshot(memento);
            author.Should().BeEquivalentTo(_author);
        }

        [SetUp]
        protected void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Author>())
                .Returns(new AuthorMemento());

            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            _fixture = new Fixture();

            _author = (Author) ReflectionHelper.CreateInstance(typeof(Author));

            _author.ApplyChange(_fixture.Create<AuthorCreated>().WithAggregate(_fixture.Create<Guid>())
                .WithFullName(_fixture.Create<string>(), _fixture.Create<string>()).WithAddedBy(_fixture.Create<Guid>())
                .WithSex(Sex.Male.Value));

            _author.CommitEvents();
        }
    }
}