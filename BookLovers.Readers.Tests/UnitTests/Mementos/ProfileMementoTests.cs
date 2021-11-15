using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Domain.Profiles;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Infrastructure.Mementos;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.SharedSexes;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Mementos
{
    public class ProfileMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private Profile _profile;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_profile);
            var profile = (Profile) ReflectionHelper.CreateInstance(typeof(Profile));

            var memento = _mementoFactoryMock.Object.Create<Profile>();
            memento = (IMemento<Profile>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                memento.GetType(), SerializerSettings.GetSerializerSettings());

            profile.ApplySnapshot(memento);
            profile.Should().NotBeNull();
            profile.Should().NotBe(null);
            profile.Should().BeEquivalentTo(_profile);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_profile);

            snapshot.AggregateGuid.Should().Be(_profile.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<Profile>())
                .Returns(new SocialProfileMemento());

            _fixture = new Fixture();
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);
            _profile = new Profile(_fixture.Create<Guid>(), _fixture.Create<Guid>(), _fixture.Create<DateTime>());

            for (var index = 0; index < 5; ++index)
            {
                _profile.ApplyChange(FavouriteAuthorAdded.Initialize()
                    .WithAggregate(_profile.Guid)
                    .WithAuthor(_fixture.Create<Guid>())
                    .WithReader(_profile.ReaderGuid)
                    .WithFavouriteType(FavouriteType.FavouriteAuthor.Value));

                _profile.ApplyChange(FavouriteBookAdded.Initialize()
                    .WithAggregate(_profile.Guid)
                    .WithReader(_profile.ReaderGuid).WithBook(_fixture.Create<Guid>())
                    .WithFavouriteType(FavouriteType.FavouriteBook.Value));
            }

            _profile.ChangeAddress(new Address(_fixture.Create<string>(), _fixture.Create<string>()));

            _profile.ChangeIdentity(
                new Identity(_fixture.Create<string>(), Sex.Male.Value, _fixture.Create<DateTime>()));
        }
    }
}