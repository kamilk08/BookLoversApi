using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class NewAuthorBookShouldBeDistinct : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book already contains selected author";

        private readonly Book _book;
        private readonly BookAuthor _bookAuthor;

        public NewAuthorBookShouldBeDistinct(Book book, BookAuthor bookAuthor)
        {
            this._book = book;
            this._bookAuthor = bookAuthor;
        }

        public bool IsFulfilled()
        {
            return !this._book.Authors.Contains(this._bookAuthor);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}