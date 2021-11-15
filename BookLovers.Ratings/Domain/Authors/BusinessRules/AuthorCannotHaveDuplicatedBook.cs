using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Authors.BusinessRules
{
    internal class AuthorCannotHaveDuplicatedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author cannot have duplicated books.";

        private readonly Author _author;
        private readonly Book _book;

        public AuthorCannotHaveDuplicatedBook(Author author, Book book)
        {
            this._author = author;
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return !this._author.Books.Contains(this._book);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}