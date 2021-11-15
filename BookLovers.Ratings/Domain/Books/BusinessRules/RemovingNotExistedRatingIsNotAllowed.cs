using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class RemovingNotExistedRatingIsNotAllowed : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Cannot remove rating that has not been added by reader";

        private readonly Book _book;
        private readonly Rating _rating;

        public RemovingNotExistedRatingIsNotAllowed(Book book, Rating rating)
        {
            this._book = book;
            this._rating = rating;
        }

        public bool IsFulfilled()
        {
            return this._book.Ratings.Contains(this._rating);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}