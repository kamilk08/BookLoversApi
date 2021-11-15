using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Likes;

namespace BookLovers.Publication.Domain.Quotes.BusinessRules
{
    internal class QuoteCannotBeLikedMultipleTimesBySameReader : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Quote cannot be liked multiple times by same reader";

        private readonly Quote _quote;
        private readonly Like _like;

        public QuoteCannotBeLikedMultipleTimesBySameReader(Quote quote, Like like)
        {
            this._quote = quote;
            this._like = like;
        }

        public bool IsFulfilled() => !this._quote.Likes.Contains(this._like);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}