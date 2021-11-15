using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorDetailsChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public string BirthPlace { get; private set; }

        public DateTime? BirthDate { get; private set; }

        public DateTime? DeathDate { get; private set; }

        public Guid AggregateGuid { get; private set; }

        private AuthorDetailsChanged()
        {
        }

        private AuthorDetailsChanged(
            Guid authorGuid,
            string birthPlace,
            DateTime? birthDate,
            DateTime? deathDate)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = authorGuid;
            this.BirthPlace = birthPlace;
            this.BirthDate = birthDate;
            this.DeathDate = deathDate;
        }

        public static AuthorDetailsChanged Initialize()
        {
            return new AuthorDetailsChanged();
        }

        public AuthorDetailsChanged WithAggregate(Guid aggregateGuid)
        {
            return new AuthorDetailsChanged(aggregateGuid, this.BirthPlace, this.BirthDate, this.DeathDate);
        }

        public AuthorDetailsChanged WithBirthPlace(string birthPlace)
        {
            return new AuthorDetailsChanged(this.AggregateGuid, birthPlace, this.BirthDate, this.DeathDate);
        }

        public AuthorDetailsChanged WithDates(
            DateTime? birthDate,
            DateTime? deathDate)
        {
            return new AuthorDetailsChanged(this.AggregateGuid, this.BirthPlace, birthDate, deathDate);
        }
    }
}