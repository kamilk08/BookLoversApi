using System;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class ShelfCannotHaveMultipleSameBooks : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelf already contains selected book.";

        private readonly Shelf _shelf;
        private readonly Guid _bookGuid;

        public ShelfCannotHaveMultipleSameBooks(Shelf shelf, Guid bookGuid)
        {
            _shelf = shelf;
            _bookGuid = bookGuid;
        }

        public bool IsFulfilled() => !_shelf.Books.Contains(_bookGuid);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}