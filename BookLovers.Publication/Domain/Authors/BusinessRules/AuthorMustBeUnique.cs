using System;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Authors.Services;

namespace BookLovers.Publication.Domain.Authors.BusinessRules
{
    internal class AuthorMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Author is not unique";

        private readonly IAuthorUniquenessChecker _checker;
        private readonly Guid _guid;

        public AuthorMustBeUnique(IAuthorUniquenessChecker checker, Guid guid)
        {
            this._checker = checker;
            this._guid = guid;
        }

        public bool IsFulfilled() => this._checker.IsUnique(this._guid);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}