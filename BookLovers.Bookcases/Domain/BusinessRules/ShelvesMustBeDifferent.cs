using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class ShelvesMustBeDifferent : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelves need to be different.";

        private readonly Shelf _firstShelf;
        private readonly Shelf _secondShelf;

        public ShelvesMustBeDifferent(Shelf firstShelf, Shelf secondShelf)
        {
            _firstShelf = firstShelf;
            _secondShelf = secondShelf;
        }

        public bool IsFulfilled() => _firstShelf.Guid != _secondShelf.Guid;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}