using System;
using AutoFixture;
using BookLovers.Base.Domain;
using BookLovers.Base.Infrastructure.Extensions;
using BookLovers.Base.Infrastructure.Persistence;
using BookLovers.Base.Infrastructure.Serialization;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Events.Settings;
using BookLovers.Bookcases.Infrastructure.Mementos;
using BookLovers.Bookcases.Store.Persistence;
using BookLovers.Shared.Privacy;
using FluentAssertions;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BookLovers.Bookcases.Tests.UnitTests
{
    public class SettingsManagerMementoTests
    {
        private readonly Mock<IMementoFactory> _mementoFactoryMock =
            new Mock<IMementoFactory>();

        private ISnapshotMaker _snapshotMaker;
        private Fixture _fixture;
        private SettingsManager _settingsManager;

        [Test]
        public void ApplySnapshot_WhenCalled_ShouldApplySnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_settingsManager);
            var settingsManager = (SettingsManager) ReflectionHelper.CreateInstance(typeof(SettingsManager));
            var settingsManagerMemento = _mementoFactoryMock.Object.Create<SettingsManager>();

            settingsManagerMemento = (IMemento<SettingsManager>) JsonConvert.DeserializeObject(
                snapshot.SnapshottedAggregate,
                settingsManagerMemento.GetType(), SerializerSettings.GetSerializerSettings());

            settingsManager.ApplySnapshot(settingsManagerMemento);
            settingsManager.Should().NotBe(null);
            settingsManager.Should().BeEquivalentTo(_settingsManager);
            settingsManager.Options.Should().Equal(_settingsManager.Options);
        }

        [Test]
        public void MakeSnapshot_WhenCalled_ShouldCreateSnapshotOfSelectedAggregate()
        {
            var snapshot = _snapshotMaker.MakeSnapshot(_settingsManager);

            snapshot.AggregateGuid.Should().Be(_settingsManager.Guid);

            snapshot.SnapshottedAggregate.Should().NotBeEmpty();
        }

        [SetUp]
        public void SetUp()
        {
            _mementoFactoryMock
                .Setup(s => s.Create<SettingsManager>())
                .Returns(new SettingsManagerMemento());

            _snapshotMaker = new SnapshotMaker(_mementoFactoryMock.Object);

            _fixture = new Fixture();

            _settingsManager = new SettingsManager(_fixture.Create<Guid>(), _fixture.Create<Guid>());

            _settingsManager.ApplyChange(new PrivacyOptionChanged(
                _settingsManager.Guid,
                _settingsManager.BookcaseGuid, PrivacyOption.OtherReaders.Value));

            for (var capacity = 10; capacity < 20; ++capacity)
                _settingsManager.ApplyChange(new ShelfCapacityChanged(
                    _settingsManager.Guid,
                    _settingsManager.BookcaseGuid, capacity));
        }
    }
}