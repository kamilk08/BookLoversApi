using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Publishers.Services;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class PublisherMustBeUnique : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher is not unique.";

        private readonly IPublisherUniquenessChecker _checker;
        private readonly Publisher _publisher;

        public PublisherMustBeUnique(IPublisherUniquenessChecker checker, Publisher publisher)
        {
            this._checker = checker;
            this._publisher = publisher;
        }

        public bool IsFulfilled() => this._checker.IsUnique(this._publisher.Guid, this._publisher.PublisherName);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}