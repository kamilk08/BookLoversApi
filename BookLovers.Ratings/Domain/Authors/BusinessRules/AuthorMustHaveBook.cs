using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Authors.BusinessRules
{
    internal class AuthorMustHaveBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author does not have selected book.";

        private readonly Author _author;
        private readonly Book _book;

        public AuthorMustHaveBook(Author author, Book book)
        {
            this._author = author;
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._author.Books.Any(a => a.Identification.BookGuid
                                               == this._book.Identification.BookGuid);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}