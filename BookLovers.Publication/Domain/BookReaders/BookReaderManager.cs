using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.BookReaders;

namespace BookLovers.Publication.Domain.BookReaders
{
    internal class BookReaderManager : IAggregateManager<BookReader>
    {
        private readonly List<Func<BookReader, IBusinessRule>> _rules =
            new List<Func<BookReader, IBusinessRule>>();

        public BookReaderManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(BookReader aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new BookReaderSuspended(aggregate.Guid, aggregate.ReaderId));
        }
    }
}