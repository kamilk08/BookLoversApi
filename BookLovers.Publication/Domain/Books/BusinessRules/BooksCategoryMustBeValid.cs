using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Books.BusinessRules
{
    internal class BooksCategoryMustBeValid : IBusinessRule
    {
        private const string BrokenBusinessRuleMessage = "Invalid book category";
        private readonly BookCategory _bookCategory;

        public BooksCategoryMustBeValid(BookCategory category)
        {
            this._bookCategory = category;
        }

        public bool IsFulfilled()
        {
            return CategoryList.Categories.Contains(this._bookCategory.Category);
        }

        public string BrokenRuleMessage => BrokenBusinessRuleMessage;
    }
}