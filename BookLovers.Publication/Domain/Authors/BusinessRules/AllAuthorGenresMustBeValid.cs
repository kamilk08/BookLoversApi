using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.Categories;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AllAuthorGenresMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "One of the author genres is invalid.";

        private readonly Author _author;

        public AllAuthorGenresMustBeValid(Author author)
        {
            this._author = author;
        }

        public bool IsFulfilled()
        {
            return this._author.Genres.All(a => SubCategoryList.SubCategories.Contains(a));
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}