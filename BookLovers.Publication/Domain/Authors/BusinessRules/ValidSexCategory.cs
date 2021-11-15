using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class ValidSexCategory : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Invalid sex category";

        private readonly Sex _sex;

        public ValidSexCategory(Sex sex)
        {
            this._sex = sex;
        }

        public bool IsFulfilled() => Sexes.Has(this._sex);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}