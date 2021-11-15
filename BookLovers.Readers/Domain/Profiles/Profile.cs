using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles.BusinessRules;
using BookLovers.Readers.Events.Profile;
using BookLovers.Readers.Mementos;
using BookLovers.Shared;

namespace BookLovers.Readers.Domain.Profiles
{
    [AllowSnapshot]
    public class Profile :
        EventSourcedAggregateRoot,
        IHandle<ProfileCreated>,
        IHandle<ProfileAboutChanged>,
        IHandle<AddressChanged>,
        IHandle<IdentityChanged>,
        IHandle<ProfileArchived>,
        IHandle<ProfileRestored>,
        IHandle<FavouriteBookAdded>,
        IHandle<FavouriteAuthorAdded>,
        IHandle<FavouriteBookRemoved>,
        IHandle<FavouriteAuthorRemoved>,
        IHandle<CurrentRoleChanged>
    {
        private List<IFavourite> _favourites = new List<IFavourite>();

        public IReadOnlyList<IFavourite> Favourites => _favourites;

        public Guid ReaderGuid { get; private set; }

        public Address Address { get; private set; }

        public Identity Identity { get; private set; }

        public About About { get; private set; }

        public CurrentRole CurrentRole { get; private set; }

        protected Profile()
        {
        }

        public Profile(Guid profileGuid, Guid readerGuid, DateTime joinedAt)
        {
            Guid = profileGuid;
            ReaderGuid = readerGuid;
            Address = Address.Default();
            Identity = Identity.Default();
            About = About.Default(joinedAt);
            CurrentRole = CurrentRole.User();

            var @event = ProfileCreated.Initialize()
                .WithAggregate(Guid)
                .WithReader(readerGuid)
                .WithJoinedAt(About.JoinedAt)
                .WithCurrentRole(CurrentRole.RoleName);

            ApplyChange(@event);
        }

        public void ChangeAddress(Address address)
        {
            CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus != null
                ? this.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            ApplyChange(new AddressChanged(Guid, address.Country, address.City));
        }

        public void ChangeIdentity(Identity identity)
        {
            CheckBusinessRules(new ChangeIdentityRules(this, identity.Sex));

            var @event = IdentityChanged.Initialize()
                .WithAggregate(Guid)
                .WithFirstName(identity.FullName.FirstName)
                .WithSecondName(identity.FullName.SecondName)
                .WithName(identity.FullName.GetFullName())
                .WithSex(identity.Sex.Value, identity.Sex.Name)
                .WithBirthDate(identity.BirthDate);

            ApplyChange(@event);
        }

        public void ChangeAbout(About about)
        {
            CheckBusinessRules(new AggregateMustBeActive(this.AggregateStatus != null
                ? this.AggregateStatus.Value
                : AggregateStatus.Archived.Value));

            ApplyChange(new ProfileAboutChanged(Guid, about.WebSite, about.AboutYourself));
        }

        public void ChangeRole(string role)
        {
            ApplyChange(new CurrentRoleChanged(Guid, role));
        }

        private void RemoveFavourite(Guid favouriteGuid)
        {
            _favourites.Remove(GetFavourite(favouriteGuid));
        }

        public IFavourite GetFavourite(Guid favouriteGuid)
        {
            return _favourites.Find(p => p.FavouriteGuid == favouriteGuid);
        }

        public bool HasFavourite(Guid favouriteGuid)
        {
            return GetFavourite(favouriteGuid) != null;
        }

        void IHandle<ProfileAboutChanged>.Handle(ProfileAboutChanged @event)
        {
            About = new About(About.JoinedAt, @event.WebSite, @event.About);
        }

        void IHandle<AddressChanged>.Handle(AddressChanged @event)
        {
            Address = new Address(@event.Country, @event.City);
        }

        void IHandle<IdentityChanged>.Handle(IdentityChanged @event)
        {
            Identity =
                new Identity(new FullName(@event.FirstName, @event.SecondName), @event.Sex, @event.BirthDate);
        }

        void IHandle<ProfileCreated>.Handle(ProfileCreated @event)
        {
            Guid = @event.AggregateGuid;
            AggregateStatus = AggregateStatus.Active;
            ReaderGuid = @event.ReaderGuid;
            Identity = Identity.Default();
            Address = Address.Default();
            About = About.Default(@event.JoinedAt);
            CurrentRole = CurrentRole.User();
        }

        void IHandle<ProfileArchived>.Handle(ProfileArchived @event)
        {
            AggregateStatus = AggregateStatus.Archived;
        }

        void IHandle<ProfileRestored>.Handle(ProfileRestored @event)
        {
            AggregateStatus = AggregateStatus.Active;
        }

        void IHandle<FavouriteBookAdded>.Handle(FavouriteBookAdded @event)
        {
            _favourites.Add(new FavouriteBook(@event.BookGuid));
        }

        void IHandle<FavouriteAuthorRemoved>.Handle(FavouriteAuthorRemoved @event)
        {
            RemoveFavourite(@event.AuthorGuid);
        }

        void IHandle<FavouriteBookRemoved>.Handle(FavouriteBookRemoved @event)
        {
            RemoveFavourite(@event.BookGuid);
        }

        void IHandle<FavouriteAuthorAdded>.Handle(FavouriteAuthorAdded @event)
        {
            _favourites.Add(new FavouriteAuthor(@event.AuthorGuid));
        }

        void IHandle<CurrentRoleChanged>.Handle(CurrentRoleChanged @event)
        {
            CurrentRole = new CurrentRole(@event.CurrentRole);
        }

        public override void ApplySnapshot(IMemento memento)
        {
            var socialProfileMemento = memento as ISocialProfileMemento;

            Guid = socialProfileMemento.AggregateGuid;
            AggregateStatus = AggregateStates.Get(socialProfileMemento.AggregateStatus);
            Version = socialProfileMemento.Version;
            LastCommittedVersion = socialProfileMemento.LastCommittedVersion;

            ReaderGuid = socialProfileMemento.ReaderGuid;
            Address = new Address(socialProfileMemento.Country, socialProfileMemento.City);
            Identity = new Identity(
                new FullName(socialProfileMemento.FirstName, socialProfileMemento.SecondName),
                socialProfileMemento.Sex, socialProfileMemento.BirthDate);
            About = new About(socialProfileMemento.JoinedAt, socialProfileMemento.WebSite, socialProfileMemento.About);

            CurrentRole = new CurrentRole(socialProfileMemento.CurrentRole);

            _favourites = socialProfileMemento.Favourites.ToList();
        }
    }
}