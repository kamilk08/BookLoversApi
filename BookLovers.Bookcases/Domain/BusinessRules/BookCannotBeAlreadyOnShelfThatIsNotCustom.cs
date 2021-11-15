using System;
using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.ShelfCategories;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class BookCannotBeAlreadyOnShelfThatIsNotCustom : IBusinessRule
    {
        private const string BrokenErrorRuleMessage = "Book cannot have same book on multiple core shelfs";

        private readonly Bookcase _bookcase;
        private readonly Shelf _shelf;
        private readonly Guid _bookGuid;

        public BookCannotBeAlreadyOnShelfThatIsNotCustom(Bookcase bookcase, Shelf shelf, Guid bookGuid)
        {
            _bookcase = bookcase;
            _shelf = shelf;
            _bookGuid = bookGuid;
        }

        public bool IsFulfilled()
        {
            return _shelf.ShelfDetails.Category == ShelfCategory.Custom
                   || !_bookcase.Shelves
                       .Where(p => p.ShelfDetails.Category != ShelfCategory.Custom)
                       .Any(a => a.Books.Contains(_bookGuid));
        }

        public string BrokenRuleMessage => BrokenErrorRuleMessage;
    }
}