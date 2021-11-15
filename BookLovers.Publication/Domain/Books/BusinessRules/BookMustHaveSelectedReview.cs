using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookMustHaveSelectedReview : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book has no review from selected reader";

        private readonly Book _book;
        private readonly BookReview _bookReview;

        public BookMustHaveSelectedReview(Book book, BookReview bookReview)
        {
            this._book = book;
            this._bookReview = bookReview;
        }

        public bool IsFulfilled()
        {
            return this._book.Reviews.Contains(this._bookReview);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}