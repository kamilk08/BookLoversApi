using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorDescriptionChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string AboutAuthor { get; private set; }

        public string DescriptionSource { get; private set; }

        public string AuthorWebsite { get; private set; }

        private AuthorDescriptionChanged()
        {
        }

        private AuthorDescriptionChanged(
            Guid authorGuid,
            string aboutAuthor,
            string descriptionSource,
            string authorWebsite)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = authorGuid;
            this.AboutAuthor = aboutAuthor;
            this.DescriptionSource = descriptionSource;
            this.AuthorWebsite = authorWebsite;
        }

        public static AuthorDescriptionChanged Initialize()
        {
            return new AuthorDescriptionChanged();
        }

        public AuthorDescriptionChanged WithAggregate(Guid aggregateGuid)
        {
            return new AuthorDescriptionChanged(
                aggregateGuid,
                this.AboutAuthor, this.DescriptionSource, this.AuthorWebsite);
        }

        public AuthorDescriptionChanged WithDescription(
            string description,
            string descriptionSource)
        {
            return new AuthorDescriptionChanged(this.AggregateGuid, description, descriptionSource, this.AuthorWebsite);
        }

        public AuthorDescriptionChanged WithWebSite(string website)
        {
            return new AuthorDescriptionChanged(this.AggregateGuid, this.AboutAuthor, this.DescriptionSource, website);
        }
    }
}