using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Publishers.BusinessRules;

namespace BookLovers.Publication.Domain.Publishers.Services
{
    public class PublisherFactory
    {
        private readonly List<Func<Publisher, IBusinessRule>> _rules =
            new List<Func<Publisher, IBusinessRule>>();

        public PublisherFactory(IPublisherUniquenessChecker checker)
        {
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
            this._rules.Add(aggregate => new PublisherMustBeUnique(checker, aggregate));
        }

        public Publisher Create(Guid guid, string name)
        {
            var publisher = new Publisher(guid, name);

            foreach (var rule in this._rules)
            {
                if (!rule(publisher).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(publisher).BrokenRuleMessage);
            }

            return publisher;
        }
    }
}