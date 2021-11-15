using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Publication.Domain.Authors
{
    public class AuthorDetails : ValueObject<AuthorDetails>
    {
        public DateTime? BirthDate { get; }

        public DateTime? DeathDate { get; }

        public string BirthPlace { get; }

        private AuthorDetails()
        {
        }

        public AuthorDetails(string birthPlace, DateTime? birthDate, DateTime? deathDate)
        {
            this.BirthPlace = birthPlace;
            BirthDate = birthDate == default(DateTime) ? null : birthDate;
            DeathDate = deathDate == default(DateTime) ? null : deathDate;
        }

        public static AuthorDetails Default() => new AuthorDetails(string.Empty, null, null);

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.BirthDate.GetHashCode();
            hash = (hash * 23) + this.BirthPlace.GetHashCode();
            hash = (hash * 23) + this.DeathDate.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(AuthorDetails obj)
        {
            return this.BirthDate == obj.BirthDate && this.BirthPlace == obj.BirthPlace
                                                   && this.DeathDate == obj.DeathDate;
        }
    }
}