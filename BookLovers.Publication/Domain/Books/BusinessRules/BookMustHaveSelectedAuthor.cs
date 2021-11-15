using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookMustHaveSelectedAuthor : IBusinessRule
    {
        private const string BrokenRuleErrorMessage =
            "Cannot remove author from book.Book does not contain selected author";

        private readonly Book _book;
        private readonly BookAuthor _bookAuthor;

        public BookMustHaveSelectedAuthor(Book book, BookAuthor bookAuthor)
        {
            this._book = book;
            this._bookAuthor = bookAuthor;
        }

        public bool IsFulfilled()
        {
            return this._book.Authors.Contains(this._bookAuthor);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}