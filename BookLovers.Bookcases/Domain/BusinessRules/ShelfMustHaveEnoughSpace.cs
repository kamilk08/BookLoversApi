using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.Settings;
using BookLovers.Bookcases.Domain.ShelfCategories;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class ShelfMustHaveEnoughSpace : IBusinessRule
    {
        private const string BrokenRuleErrorMessage =
            "Shelf either does not exist or there is not enough enough space.";

        private readonly SettingsManager _settingsManager;
        private readonly Shelf _shelf;

        public ShelfMustHaveEnoughSpace(SettingsManager settingsManager, Shelf shelf)
        {
            _settingsManager = settingsManager;
            _shelf = shelf;
        }

        public bool IsFulfilled()
        {
            var shelfCapacity =
                _settingsManager.Options.Single(p => p.Type == BookcaseOptionType.ShelfCapacity) as ShelfCapacity;
            if (shelfCapacity == null || _shelf == null)
                return false;

            return _shelf.ShelfDetails.Category != ShelfCategory.Custom ||
                   !shelfCapacity.CurrentCapacityExceeded(_shelf);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}