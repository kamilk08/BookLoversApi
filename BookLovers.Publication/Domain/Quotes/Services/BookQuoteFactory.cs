using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Books;
using BookLovers.Publication.Domain.Books.BusinessRules;
using BookLovers.Publication.Domain.Quotes.BusinessRules;
using BookLovers.Publication.Events.Quotes;

namespace BookLovers.Publication.Domain.Quotes.Services
{
    internal class BookQuoteFactory : IQuoteFactory
    {
        private readonly List<Func<Quote, Book, IBusinessRule>> _rules =
            new List<Func<Quote, Book, IBusinessRule>>();

        private readonly IUnitOfWork _unitOfWork;

        public QuoteType QuoteType => QuoteType.BookQuote;

        public BookQuoteFactory(IUnitOfWork unitOfWork, IQuoteUniquenessChecker checker)
        {
            this._unitOfWork = unitOfWork;

            this._rules.Add((quote, book) => new BookMustBePresent(book));
            this._rules.Add((quote, book) => new AggregateMustBeActive(quote.AggregateStatus.Value));
            this._rules.Add((quote, book) => new QuoteMustHaveQuotedObjectGuid(quote));
            this._rules.Add((quote, book) => new QuoteMustBeUnique(checker, quote.Guid));
        }

        public async Task<Quote> Create(
            Guid quoteGuid,
            QuoteContent quoteContent,
            QuoteDetails quoteDetails)
        {
            var book = await this._unitOfWork.GetAsync<Book>(quoteContent.QuotedGuid);
            var quote = new Quote(quoteGuid, quoteContent, quoteDetails);

            var @event = BookQuoteAdded.Initialize()
                .WithAggregate(quote.Guid)
                .WithBook(quote.QuoteContent.QuotedGuid)
                .WithQuote(quote.QuoteContent.Content, quote.QuoteDetails.AddedAt)
                .WithAddedBy(quote.QuoteDetails.AddedByGuid);

            quote.ApplyChange(@event);

            foreach (var rule in this._rules)
            {
                if (!rule(quote, book).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(quote, book).BrokenRuleMessage);
            }

            return quote;
        }
    }
}