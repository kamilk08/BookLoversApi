using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Likes;

namespace BookLovers.Publication.Domain.Quotes.BusinessRules
{
    internal class QuoteMustContainSelectedLike : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Quote must contain selected like.";

        private readonly Quote _quote;
        private readonly Like _like;

        public QuoteMustContainSelectedLike(Quote quote, Like like)
        {
            this._quote = quote;
            this._like = like;
        }

        public bool IsFulfilled() => this._quote.Likes.Contains(this._like);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}