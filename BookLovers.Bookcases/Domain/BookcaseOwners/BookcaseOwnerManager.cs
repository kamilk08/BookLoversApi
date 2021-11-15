using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Events.Readers;

namespace BookLovers.Bookcases.Domain.BookcaseOwners
{
    public class BookcaseOwnerManager : IAggregateManager<BookcaseOwner>
    {
        private readonly List<Func<BookcaseOwner, IBusinessRule>> _rules =
            new List<Func<BookcaseOwner, IBusinessRule>>();

        public BookcaseOwnerManager()
        {
            _rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            _rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(BookcaseOwner aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new BookcaseOwnerArchived(aggregate.Guid, aggregate.BookcaseGuid));
        }
    }
}