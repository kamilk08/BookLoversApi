using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.BusinessRules
{
    internal class ShelfWithGivenNameAlreadyExists : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Shelf with given name already exists";

        private readonly Bookcase _bookcase;
        private readonly string _shelfName;

        public ShelfWithGivenNameAlreadyExists(Bookcase bookcase, string shelfName)
        {
            _bookcase = bookcase;
            _shelfName = shelfName;
        }

        public bool IsFulfilled() =>
            _bookcase.Shelves.Any(a => a.ShelfDetails.ShelfName != _shelfName);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}