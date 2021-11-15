using System;
using System.Linq;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Readers.Domain.ProfileManagers;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Readers.Events.ProfileManagers;
using BookLovers.Readers.Infrastructure.Mementos;
using BookLovers.Readers.Store.Persistence;
using BookLovers.Shared.Privacy;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Readers.Tests.UnitTests.Mementos
{
    public class PrivacyManagerMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock = new Mock<IMementoFactory>();
        private ProfilePrivacyManager _manager;
        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_manager);
            var manager = (ProfilePrivacyManager) ReflectionHelper.CreateInstance(typeof(ProfilePrivacyManager));
            var memento = _mementoFactoryMock.Object.Create<ProfilePrivacyManager>();
            memento = (IMemento<ProfilePrivacyManager>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate, memento.GetType(), SerializerSettings.GetSerializerSettings());

            manager.ApplySnapshot(memento);
            manager.Should().NotBeNull();
            manager.Should().NotBe(null);
            manager.Should().BeEquivalentTo(_manager);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_manager);

            snapshot.AggregateGuid.Should().Be(_manager.Guid);
            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock.Setup(s => s.Create<ProfilePrivacyManager>())
                .Returns(new PrivacyManagerMemento());

            _fixture = new Fixture();
            _manager = new ProfilePrivacyManager(_fixture.Create<Guid>(), _fixture.Create<Guid>());
            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            var addressPrivacy = ProfilePrivacyOptionAdded.Initialize().WithAggregate(_manager.Guid)
                .WithOption(PrivacyOption.Public.Value, PrivacyOption.Public.Name)
                .WithPrivacyType(ProfilePrivacyType.AddressPrivacy.Value, ProfilePrivacyType.AddressPrivacy.Name);

            var identityPrivacy = ProfilePrivacyOptionAdded.Initialize().WithAggregate(_manager.Guid)
                .WithOption(PrivacyOption.OtherReaders.Value, PrivacyOption.OtherReaders.Name)
                .WithPrivacyType(ProfilePrivacyType.IdentityPrivacy.Value, ProfilePrivacyType.IdentityPrivacy.Name);

            _manager.ApplyChange(addressPrivacy);
            _manager.ApplyChange(identityPrivacy);

            Enumerable.Range(0, 20).ForEach(i =>
            {
                _manager.ChangePrivacy(new SelectedProfileOption(
                    PrivacyOption.Public,
                    ProfilePrivacyType.AddressPrivacy));
                _manager.ChangePrivacy(new SelectedProfileOption(
                    PrivacyOption.OtherReaders,
                    ProfilePrivacyType.IdentityPrivacy));
            });
        }
    }
}