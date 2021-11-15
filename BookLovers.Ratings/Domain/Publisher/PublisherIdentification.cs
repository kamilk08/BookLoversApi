using System;
using BookLovers.Base.Domain.ValueObject;

namespace BookLovers.Ratings.Domain.Publisher
{
    public class PublisherIdentification : ValueObject<PublisherIdentification>
    {
        public Guid PublisherGuid { get; private set; }

        public int PublisherId { get; private set; }

        private PublisherIdentification()
        {
        }

        public PublisherIdentification(Guid publisherGuid, int publisherId)
        {
            PublisherGuid = publisherGuid;
            PublisherId = publisherId;
        }

        protected override int GetHashCodeCore()
        {
            var hash = 17;

            hash = (hash * 23) + this.PublisherId.GetHashCode();
            hash = (hash * 23) + this.PublisherGuid.GetHashCode();

            return hash;
        }

        protected override bool EqualsCore(PublisherIdentification obj)
        {
            return PublisherGuid == obj.PublisherGuid
                   && PublisherId == obj.PublisherId;
        }
    }
}