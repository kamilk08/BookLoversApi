using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BookCannotContainDuplicatedAuthors : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Book cannot contain duplicated authors";

        private const int SingleAuthor = 1;
        private readonly Book _book;

        public BookCannotContainDuplicatedAuthors(Book book)
        {
            this._book = book;
        }

        public bool IsFulfilled()
        {
            return this._book.Authors.GroupBy(p => p.AuthorGuid)
                .All(a => a.Count() == SingleAuthor);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}