using System.Linq;
using BookLovers.Base.Domain.Rules;
using BookLovers.Ratings.Domain.Books;

namespace BookLovers.Ratings.Domain.Publisher.BusinessRules
{
    internal class PublisherMustHaveSelectedBook : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Publisher does not contain selected book.";

        private readonly Publisher _publisher;
        private readonly Book _book;

        public PublisherMustHaveSelectedBook(Publisher publisher, Book book)
        {
            _publisher = publisher;
            _book = book;
        }

        public bool IsFulfilled()
        {
            return _publisher.Books.Contains(_book);
        }

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}