using System.Linq;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Publication.Domain.Publishers.BusinessRules
{
    internal class PublisherCannotHaveDuplicatedBooks : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher cannot have duplicated books.";

        private readonly Publisher _publisher;
        private readonly PublisherBook _publisherBook;

        public PublisherCannotHaveDuplicatedBooks(Publisher publisher, PublisherBook publisherBook)
        {
            this._publisher = publisher;
            this._publisherBook = publisherBook;
        }

        public bool IsFulfilled() => !this._publisher.Books.Contains(this._publisherBook);

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}