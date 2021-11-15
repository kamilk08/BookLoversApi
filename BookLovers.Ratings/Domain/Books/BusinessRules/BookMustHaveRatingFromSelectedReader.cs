using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.RatingGivers;

namespace BookLovers.Ratings.Domain.Books.BusinessRules
{
    internal class BookMustHaveRatingFromSelectedReader : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book does not have selected rating.";

        private readonly RatingGiver _ratingGiver;
        private readonly Book _book;

        public BookMustHaveRatingFromSelectedReader(Book book, RatingGiver ratingGiver)
        {
            this._ratingGiver = ratingGiver;
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._ratingGiver != null
                   && this._book.Ratings.Any(a => a.ReaderId == this._ratingGiver.ReaderId);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}