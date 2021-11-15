using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorCannotContainDuplicatedBooks : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author cannot contain duplicated books.";

        private readonly Author _author;
        private readonly AuthorBook _authorBook;

        public AuthorCannotContainDuplicatedBooks(Author author, AuthorBook authorBook)
        {
            this._author = author;
            this._authorBook = authorBook;
        }

        public bool IsFulfilled() =>
            !this._author.Books.Contains(this._authorBook);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}