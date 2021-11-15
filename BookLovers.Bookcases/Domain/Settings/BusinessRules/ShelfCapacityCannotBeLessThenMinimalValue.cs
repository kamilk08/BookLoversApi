using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.Settings.BusinessRules
{
    internal class ShelfCapacityCannotBeLessThenMinimalValue : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelf capacity cannot be less then minimal shelf capacity.";

        private readonly ShelfCapacity _shelfCapacity;
        private readonly int _selectedCapacity;

        public ShelfCapacityCannotBeLessThenMinimalValue(
            ShelfCapacity shelfCapacity,
            int selectedCapacity)
        {
            _shelfCapacity = shelfCapacity;
            _selectedCapacity = selectedCapacity;
        }

        public bool IsFulfilled() =>
            _selectedCapacity >= ShelfCapacity.MinCapacity;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}