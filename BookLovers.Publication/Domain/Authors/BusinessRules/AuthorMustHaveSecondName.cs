using BookLovers.Base.Domain.Rules;
using BookLovers.Shared;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorMustHaveSecondName : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author must have secondName";

        private readonly FullName _fullName;

        public AuthorMustHaveSecondName(FullName fullName)
        {
            this._fullName = fullName;
        }

        public bool IsFulfilled()
        {
            return this._fullName != null
                   && this._fullName.SecondName != string.Empty;
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}