using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorMustHaveBookInHisCollection : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author must have book in his collection.";

        private readonly Author _author;
        private readonly AuthorBook _authorBook;

        public AuthorMustHaveBookInHisCollection(Author author, AuthorBook authorBook)
        {
            this._author = author;
            this._authorBook = authorBook;
        }

        public bool IsFulfilled() => this._author.Books.Contains(this._authorBook);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}