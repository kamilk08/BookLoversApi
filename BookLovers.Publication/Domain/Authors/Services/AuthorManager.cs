using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.Authors;

namespace BookLovers.Publication.Domain.Authors.Services
{
    public class AuthorManager : IAggregateManager<Author>
    {
        private readonly List<Func<Author, IBusinessRule>> _rules = new List<Func<Author, IBusinessRule>>();

        public AuthorManager()
        {
            this._rules.Add(aggregate => new AggregateMustExist(aggregate.Guid));
            this._rules.Add(aggregate => new AggregateMustBeActive(aggregate.AggregateStatus.Value));
        }

        public void Archive(Author aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            var authorBooks = aggregate.Books
                .Select(s => s.BookGuid).AsEnumerable();

            var authorQuotes = aggregate.AuthorQuotes
                .Select(s => s.QuoteGuid).AsEnumerable();

            aggregate.ApplyChange(new AuthorArchived(aggregate.Guid, authorBooks, authorQuotes));
        }
    }
}