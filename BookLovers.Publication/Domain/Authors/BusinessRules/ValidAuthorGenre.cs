using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class ValidAuthorGenre : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid author genre.";

        private readonly SubCategory _category;

        public ValidAuthorGenre(SubCategory category)
        {
            this._category = category;
        }

        public bool IsFulfilled() =>
            SubCategoryList.SubCategories.Contains(this._category);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}