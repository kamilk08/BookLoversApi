using BookLovers.Base.Domain.Rules;
using BookLovers.Shared.SharedSexes;

namespace BookLovers.Readers.Domain.Profiles.BusinessRules
{
    internal class SexTypeMustBeValid : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Reader sex is not valid.";
        private readonly Sex _sex;

        public SexTypeMustBeValid(Sex sex)
        {
            _sex = sex;
        }

        public bool IsFulfilled()
        {
            return Sexes.Has(_sex);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}