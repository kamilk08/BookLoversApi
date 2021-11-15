using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Infrastructure;
using BookLovers.Publication.Domain.Authors;
using BookLovers.Publication.Domain.Authors.BusinessRules;
using BookLovers.Publication.Domain.Quotes.BusinessRules;
using BookLovers.Publication.Events.Quotes;

namespace BookLovers.Publication.Domain.Quotes.Services
{
    public class AuthorQuoteFactory : IQuoteFactory
    {
        private readonly List<Func<Quote, Author, IBusinessRule>> _rules =
            new List<Func<Quote, Author, IBusinessRule>>();

        private readonly IUnitOfWork _unitOfWork;

        public QuoteType QuoteType => QuoteType.AuthorQuote;

        public AuthorQuoteFactory(IUnitOfWork unitOfWork, IQuoteUniquenessChecker checker)
        {
            this._unitOfWork = unitOfWork;
            this._rules.Add((quote, author) => new AuthorMustBePresent(author));
            this._rules.Add((quote, author) => new AggregateMustBeActive(quote.AggregateStatus.Value));
            this._rules.Add((quote, author) => new QuoteMustHaveQuotedObjectGuid(quote));
            this._rules.Add((quote, author) => new QuoteMustBeUnique(checker, quote.Guid));
        }

        public async Task<Quote> Create(
            Guid quoteGuid,
            QuoteContent quoteContent,
            QuoteDetails quoteDetails)
        {
            var author = await this._unitOfWork.GetAsync<Author>(quoteContent.QuotedGuid);

            var quote = new Quote(quoteGuid, quoteContent, quoteDetails);

            var @event = AuthorQuoteAdded.Initialize()
                .WithAggregate(quote.Guid)
                .WithAuthor(quote.QuoteContent.QuotedGuid)
                .WithQuote(quote.QuoteContent.Content, quote.QuoteDetails.AddedAt)
                .WithAddedBy(quote.QuoteDetails.AddedByGuid);

            quote.ApplyChange(@event);

            foreach (var rule in this._rules)
            {
                if (!rule(quote, author).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(quote, author).BrokenRuleMessage);
            }

            return quote;
        }
    }
}