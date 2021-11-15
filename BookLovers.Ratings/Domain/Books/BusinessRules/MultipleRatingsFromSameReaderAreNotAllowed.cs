using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class MultipleRatingsFromSameReaderAreNotAllowed : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book cannot have multiple ratings from same reader.";

        private readonly Book _book;
        private readonly Rating _rating;

        public MultipleRatingsFromSameReaderAreNotAllowed(Book book, Rating rating)
        {
            this._book = book;
            this._rating = rating;
        }

        public bool IsFulfilled()
        {
            return this._book.Ratings.All(a => a.ReaderId != this._rating.ReaderId);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}