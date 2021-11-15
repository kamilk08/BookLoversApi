using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class BookcaseMustContainSelectedShelf : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Bookcase does not have selected shelf.";

        private readonly Bookcase _bookcase;
        private readonly Shelf _shelf;

        public BookcaseMustContainSelectedShelf(Bookcase bookcase, Shelf shelf)
        {
            _bookcase = bookcase;
            _shelf = shelf;
        }

        public bool IsFulfilled() => _bookcase.Shelves.Contains(_shelf);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}