using BookLovers.Base.Domain.Rules;
using BookLovers.Bookcases.Domain.ShelfCategories;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class ShelfIsOfTypeCustom : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelf type is not custom.";
        private readonly Shelf _shelf;

        public ShelfIsOfTypeCustom(Shelf shelf)
        {
            _shelf = shelf;
        }

        public bool IsFulfilled() =>
            _shelf.ShelfDetails.Category.Equals(ShelfCategory.Custom);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}