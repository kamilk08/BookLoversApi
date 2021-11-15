using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorCreated : IEvent
    {
        public string AboutAuthor { get; private set; }

        public string DescriptionSource { get; private set; }

        public string AuthorWebsite { get; private set; }

        public string FirstName { get; private set; }

        public string SecondName { get; private set; }

        public string FullName { get; private set; }

        public int Sex { get; private set; }

        public DateTime? BirthDate { get; private set; }

        public DateTime? DeathDate { get; private set; }

        public string BirthPlace { get; private set; }

        public int AuthorStatus { get; private set; }

        public string PictureUrl { get; private set; }

        public string PictureSource { get; private set; }

        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public Guid ReaderGuid { get; private set; }

        public List<int> AuthorGenres { get; private set; }

        public List<Guid> AuthorBooks { get; private set; }

        private AuthorCreated()
        {
        }

        private AuthorCreated(
            Guid authorGuid,
            Guid readerGuid,
            string firstName,
            string secondName,
            int sex)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = authorGuid;
            this.ReaderGuid = readerGuid;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.FullName = BookLovers.Shared.FullName.ToFullName(firstName, secondName);
            this.Sex = sex;
            this.AuthorStatus = AggregateStatus.Active.Value;
        }

        public static AuthorCreated Initialize()
        {
            return new AuthorCreated();
        }

        public AuthorCreated WithAggregate(Guid authorGuid)
        {
            return new AuthorCreated(authorGuid, this.ReaderGuid,
                this.FirstName, this.SecondName, this.Sex);
        }

        public AuthorCreated WithFullName(string firstName, string secondName)
        {
            return new AuthorCreated(this.AggregateGuid, this.ReaderGuid, firstName, secondName, this.Sex);
        }

        public AuthorCreated WithSex(int sex)
        {
            return new AuthorCreated(this.AggregateGuid, this.ReaderGuid, this.FirstName, this.SecondName, sex);
        }

        public AuthorCreated WithAddedBy(Guid readerGuid)
        {
            return new AuthorCreated(this.AggregateGuid, readerGuid,
                this.FirstName, this.SecondName, this.Sex);
        }

        public AuthorCreated WithDescription(
            string aboutAuthor,
            string authorWebsite,
            string descriptionSource)
        {
            this.AboutAuthor = aboutAuthor;
            this.AuthorWebsite = authorWebsite;
            this.DescriptionSource = descriptionSource;
            return this;
        }

        public AuthorCreated WithDetails(
            string birthPlace,
            DateTime? birthDate,
            DateTime? deathDate)
        {
            this.BirthDate = birthDate ?? null;
            this.DeathDate = deathDate ?? null;
            this.BirthPlace = birthPlace;
            return this;
        }
    }
}