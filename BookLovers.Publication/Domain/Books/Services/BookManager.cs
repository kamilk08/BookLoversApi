using System;
using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services
{
    public class BookManager : IAggregateManager<BookLovers.Publication.Domain.Books.Book>
    {
        private readonly List<Func<Book, IBusinessRule>> _rules =
            new List<Func<Book, IBusinessRule>>();

        public BookManager()
        {
            this._rules.Add(book => new AggregateMustExist(book.Guid));
            this._rules.Add(book => new AggregateMustBeActive(book.AggregateStatus.Value));
        }

        public void Archive(BookLovers.Publication.Domain.Books.Book aggregate)
        {
            foreach (var rule in this._rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            var authors = aggregate.Authors.Select(s => s.AuthorGuid);
            var quotes = aggregate.BookQuotes.Select(s => s.QuoteGuid);

            var bookArchived = BookArchived.Initialize()
                .WithAggregate(aggregate.Guid)
                .WithAuthors(authors)
                .WithPublisher(aggregate.Publisher.PublisherGuid)
                .WithSeries(aggregate.Series.SeriesGuid, aggregate.Series.PositionInSeries)
                .WithQuotes(quotes);

            aggregate.ApplyChange(bookArchived);
        }
    }
}