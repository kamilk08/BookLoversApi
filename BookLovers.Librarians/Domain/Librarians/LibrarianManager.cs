using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Aggregates;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Librarians.Events.Librarians;

namespace BookLovers.Librarians.Domain.Librarians
{
    internal class LibrarianManager : IAggregateManager<Librarian>
    {
        private readonly List<Func<Librarian, IBusinessRule>> _rules = new List<Func<Librarian, IBusinessRule>>();

        public LibrarianManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate?.Guid ?? Guid.Empty));

            this._rules.Add(aggregate =>
                new AggregateMustBeActive(aggregate?.Status ?? AggregateStatus.Archived.Value));
        }

        public void Archive(Librarian aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ArchiveAggregate();

            aggregate.AddEvent(new LibrarianSuspended(aggregate.Guid, aggregate.ReaderGuid));
        }
    }
}