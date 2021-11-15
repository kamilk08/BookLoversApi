using System;
using BookLovers.Base.Domain.Events;

namespace BookLovers.Publication.Events.Authors
{
    public class AuthorBasicsChanged : IEvent
    {
        public Guid Guid { get; private set; }

        public Guid AggregateGuid { get; private set; }

        public string FirstName { get; private set; }

        public string SecondName { get; private set; }

        public string FullName { get; private set; }

        public int SexId { get; private set; }

        private AuthorBasicsChanged()
        {
        }

        private AuthorBasicsChanged(
            Guid aggregateGuid,
            string firstName,
            string secondName,
            int sexId)
        {
            this.Guid = Guid.NewGuid();
            this.AggregateGuid = aggregateGuid;
            this.FirstName = firstName;
            this.SecondName = secondName;
            this.FullName = BookLovers.Shared.FullName.ToFullName(firstName, secondName);
            this.SexId = sexId;
        }

        public static AuthorBasicsChanged Initialize()
        {
            return new AuthorBasicsChanged();
        }

        public AuthorBasicsChanged WithAggregate(Guid aggregateGuid)
        {
            return new AuthorBasicsChanged(aggregateGuid, this.FirstName, this.SecondName, this.SexId);
        }

        public AuthorBasicsChanged WithFullName(string firstName, string secondName)
        {
            return new AuthorBasicsChanged(this.AggregateGuid, firstName, secondName, this.SexId);
        }

        public AuthorBasicsChanged WithSex(int sexId)
        {
            return new AuthorBasicsChanged(this.AggregateGuid, this.FirstName, this.SecondName, sexId);
        }
    }
}