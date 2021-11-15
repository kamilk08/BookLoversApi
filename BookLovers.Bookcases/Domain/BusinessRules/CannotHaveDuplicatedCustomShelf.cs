using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class CannotHaveDuplicatedCustomShelf : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Bookcase cannot have duplicated shelves.";

        private readonly Bookcase _bookcase;
        private readonly Shelf _shelf;

        public CannotHaveDuplicatedCustomShelf(Bookcase bookcase, Shelf shelf)
        {
            _bookcase = bookcase;
            _shelf = shelf;
        }

        public bool IsFulfilled() => !_bookcase.Shelves.Contains(_shelf);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}