using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.Publishers;

namespace BookLovers.Publication.Domain.Publishers.Services
{
    public class PublisherManager : IAggregateManager<Publisher>
    {
        private readonly List<Func<Publisher, IBusinessRule>> _rules =
            new List<Func<Publisher, IBusinessRule>>();

        public PublisherManager()
        {
            this._rules.Add(publisher => new AggregateMustExist(publisher.Guid));
            this._rules.Add(publisher => new AggregateMustBeActive(publisher.AggregateStatus.Value));
        }

        public void Archive(Publisher aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            var publisherArchived =
                new PublisherArchived(aggregate.Guid, aggregate.Books.Select(s => s.BookGuid).AsEnumerable());

            aggregate.ApplyChange(publisherArchived);
        }
    }
}