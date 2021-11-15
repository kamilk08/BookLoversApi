using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Readers.Domain.ProfileManagers.BusinessRules;
using BookLovers.Readers.Domain.ProfileManagers.PrivacyOptions;
using BookLovers.Readers.Domain.ProfileManagers.Services;
using BookLovers.Readers.Events.ProfileManagers;
using BookLovers.Readers.Mementos;
using BookLovers.Shared.Privacy;

namespace BookLovers.Readers.Domain.ProfileManagers
{
    [AllowSnapshot]
    public class ProfilePrivacyManager :
        EventSourcedAggregateRoot,
        IHandle<ProfilePrivacyManagerCreated>,
        IHandle<ProfilePrivacyManagerArchived>,
        IHandle<ProfilePrivacyOptionChanged>,
        IHandle<ProfilePrivacyOptionAdded>
    {
        private List<IPrivacyOption> _profileOptions = new List<IPrivacyOption>();

        public IReadOnlyList<IPrivacyOption> Options => _profileOptions;

        public Guid ProfileGuid { get; private set; }

        private ProfilePrivacyManager()
        {
        }

        public ProfilePrivacyManager(Guid managerGuid, Guid profileGuid)
        {
            Guid = managerGuid;
            ProfileGuid = profileGuid;
            AggregateStatus = AggregateStatus.Active;

            ApplyChange(new ProfilePrivacyManagerCreated(Guid, profileGuid));
        }

        internal void AddPrivacyOption(ProfilePrivacyType privacyType)
        {
            var @event = ProfilePrivacyOptionAdded
                .Initialize()
                .WithAggregate(Guid)
                .WithPrivacyType(privacyType.Value, privacyType.Name)
                .WithOption(PrivacyOption.Public.Value, PrivacyOption.Public.Name);

            ApplyChange(@event);
        }

        public void ChangePrivacy(SelectedProfileOption selectedProfileOption)
        {
            CheckBusinessRules(new ChangePrivacyBusinessRule(
                this,
                selectedProfileOption.PrivacyType,
                selectedProfileOption.PrivacyOption));

            var @event = new ProfilePrivacyOptionChanged(
                Guid,
                selectedProfileOption.PrivacyType.Value,
                selectedProfileOption.PrivacyOption.Value);

            ApplyChange(@event);
        }

        public IPrivacyOption GetPrivacyOption(ProfilePrivacyType type) =>
            _profileOptions.Find(p => p.PrivacyType == type);

        void IHandle<ProfilePrivacyManagerCreated>.Handle(
            ProfilePrivacyManagerCreated @event)
        {
            Guid = @event.AggregateGuid;
            ProfileGuid = @event.ProfileGuid;
        }

        void IHandle<ProfilePrivacyManagerArchived>.Handle(
            ProfilePrivacyManagerArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<ProfilePrivacyOptionChanged>.Handle(
            ProfilePrivacyOptionChanged @event)
        {
            var privacy = ProfilePrivates.Get(@event.PrivacyTypeId);

            GetPrivacyOption(privacy)
                .ChangeTo(@event.PrivacyOptionId);
        }

        void IHandle<ProfilePrivacyOptionAdded>.Handle(
            ProfilePrivacyOptionAdded @event)
        {
            var privacy = ProfilePrivates.Get(@event.PrivacyTypeId);

            var option = CurrentPrivacyOptions.CurrentOptions[privacy]
                .DefaultOption();

            _profileOptions.Add(option);
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var managerMemento = memento as IPrivacyManagerMemento;

            Guid = managerMemento.AggregateGuid;

            Version = managerMemento.Version;
            LastCommittedVersion = managerMemento.LastCommittedVersion;
            AggregateStatus = AggregateStates.Get(managerMemento.AggregateStatus);

            ProfileGuid = managerMemento.ProfileGuid;
            _profileOptions = managerMemento.PrivacyOptions.ToList();
        }
    }
}