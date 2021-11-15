using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Bookcases.Events.BookcaseBooks;

namespace BookLovers.Bookcases.Domain.BookcaseBooks
{
    public class BookcaseBookManager : IAggregateManager<BookcaseBook>
    {
        private readonly List<Func<BookcaseBook, IBusinessRule>> _rules =
            new List<Func<BookcaseBook, IBusinessRule>>();

        public BookcaseBookManager()
        {
            _rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            _rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(BookcaseBook aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new BookcaseBookArchived(aggregate.Guid, aggregate.BookId));
        }
    }
}