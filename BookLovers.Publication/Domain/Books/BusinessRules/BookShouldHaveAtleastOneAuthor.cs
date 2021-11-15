using System.Collections.Generic;
using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookShouldHaveAtleastOneAuthor : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Created book should have atleast one author";

        private readonly IEnumerable<BookAuthor> _bookAuthors;

        public BookShouldHaveAtleastOneAuthor(IEnumerable<BookAuthor> bookAuthors)
        {
            this._bookAuthors = bookAuthors;
        }

        public bool IsFulfilled()
        {
            return this._bookAuthors.Count() > 0;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}