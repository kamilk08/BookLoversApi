using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookCannotContainMultipleReviewsFromSameReader : IBusinessRule
    {
        private const string BrokenRuleErrorMessage =
            "Reader already added review to selected book. Cannot duplicate reviews";

        private readonly Book _book;
        private readonly BookReview _bookReview;

        public BookCannotContainMultipleReviewsFromSameReader(Book book, BookReview bookReview)
        {
            this._book = book;
            this._bookReview = bookReview;
        }

        public bool IsFulfilled()
        {
            return !this._book.Reviews.Contains(this._bookReview);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}