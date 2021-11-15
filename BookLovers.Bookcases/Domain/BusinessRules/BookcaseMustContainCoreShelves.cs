using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.ShelfCategories;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class BookcaseMustContainCoreShelves : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Bookcase does not have required shelves.";
        private readonly Bookcase _bookcase;

        public BookcaseMustContainCoreShelves(Bookcase bookcase)
        {
            _bookcase = bookcase;
        }

        public bool IsFulfilled()
        {
            foreach (var shelf in _bookcase.Shelves)
            {
                if (shelf.ShelfDetails.Category == ShelfCategory.Custom)
                    return false;
            }

            return true;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}