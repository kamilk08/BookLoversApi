using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.Settings.BusinessRules
{
    internal class ShelfCapacityCannotExceedMaximumValue : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelf capacity cannot exceed maximum value.";

        private readonly ShelfCapacity _shelfCapacity;
        private readonly int _selectedCapacity;

        public ShelfCapacityCannotExceedMaximumValue(
            ShelfCapacity shelfCapacity,
            int selectedCapacity)
        {
            _shelfCapacity = shelfCapacity;
            _selectedCapacity = selectedCapacity;
        }

        public bool IsFulfilled() =>
            _selectedCapacity <= ShelfCapacity.MaxCapacity;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}