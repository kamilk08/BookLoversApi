using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Bookcases.Domain.Settings.BusinessRules;
using BookLovers.Bookcases.Events.Settings;
using BookLovers.Bookcases.Mementos;
using BookLovers.Shared.Privacy;

namespace BookLovers.Bookcases.Domain.Settings
{
    [AllowSnapshot]
    public class SettingsManager :
        EventSourcedAggregateRoot,
        IHandle<SettingsManagerCreated>,
        IHandle<SettingsManagerArchived>,
        IHandle<PrivacyOptionChanged>,
        IHandle<ShelfCapacityChanged>
    {
        private List<IBookcaseOption> _options = new List<IBookcaseOption>();

        public Guid BookcaseGuid { get; private set; }

        public IReadOnlyList<IBookcaseOption> Options => _options;

        private SettingsManager()
        {
        }

        public SettingsManager(Guid managerGuid, Guid bookcaseGuid)
        {
            Guid = managerGuid;
            BookcaseGuid = bookcaseGuid;

            var @event = SettingsManagerCreated.Initialize().WithAggregate(Guid).WithBookcase(bookcaseGuid)
                .WithCapacityAndPrivacy(ShelfCapacity.MinCapacity, PrivacyOption.Private.Value);

            ApplyChange(@event);
        }

        internal void ChangePrivacy(BookcasePrivacy privacy)
        {
            CheckBusinessRules(new ChangePrivacyRules(this, privacy.PrivacyOption));

            if (privacy.SelectedOption == GetOption(BookcaseOptionType.Privacy).SelectedOption)
                return;

            ApplyChange(new PrivacyOptionChanged(Guid, BookcaseGuid, privacy.SelectedOption));
        }

        internal void ChangeCapacity(int capacity)
        {
            var option = GetOption(BookcaseOptionType.ShelfCapacity) as ShelfCapacity;
            CheckBusinessRules(new ChangeShelfCapacityRules(this, option, capacity));

            if (option.SelectedOption == capacity)
                return;

            ApplyChange(new ShelfCapacityChanged(Guid, BookcaseGuid, capacity));
        }

        public IBookcaseOption GetOption(BookcaseOptionType optionType) =>
            _options.Find(p => p.Type == optionType);

        void IHandle<SettingsManagerCreated>.Handle(
            SettingsManagerCreated @event)
        {
            Guid = @event.AggregateGuid;
            BookcaseGuid = @event.BookcaseGuid;
            AggregateStatus = AggregateStatus.Active;
            _options.Add(ShelfCapacity.DefaultCapacity());
            _options.Add(BookcasePrivacy.DefaultOption());
        }

        void IHandle<PrivacyOptionChanged>.Handle(PrivacyOptionChanged @event)
        {
            var option = _options.Single(p => p.Type == BookcaseOptionType.Privacy) as
                BookcasePrivacy;

            var index = _options.FindIndex(p => p.Type == option.Type);

            _options[index] = option.ChangeTo(@event.Privacy);
        }

        void IHandle<ShelfCapacityChanged>.Handle(ShelfCapacityChanged @event)
        {
            var shelfCapacity =
                _options.Single(p => p.Type == BookcaseOptionType.ShelfCapacity) as
                    ShelfCapacity;

            var index = _options.FindIndex(p => p.Type == BookcaseOptionType.ShelfCapacity);

            _options[index] = shelfCapacity.SetCapacity(@event.Capacity);
        }

        void IHandle<SettingsManagerArchived>.Handle(
            SettingsManagerArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var settingsManagerMemento = memento as ISettingsManagerMemento;

            Version = settingsManagerMemento.Version;
            LastCommittedVersion = settingsManagerMemento.LastCommittedVersion;
            AggregateStatus = AggregateStates.Get(settingsManagerMemento.AggregateStatus);
            Guid = settingsManagerMemento.AggregateGuid;

            BookcaseGuid = settingsManagerMemento.BookcaseGuid;
            _options = settingsManagerMemento.SelectedOptions.ToList();
        }
    }
}