using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookDescriptionEditor : IBookEditor
    {
        private readonly List<Func<Book, IBusinessRule>> _rules;

        public BookDescriptionEditor()
        {
            _rules = new List<Func<Book, IBusinessRule>>();

            _rules.Add(book => new AggregateMustBeActive(book.AggregateStatus.Value));
        }

        public Task EditBook(Book book, BookData bookData)
        {
            foreach (var rule in _rules)
            {
                if (!rule(book).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(book).BrokenRuleMessage);
            }

            var description = new Description(
                bookData.DescriptionData.Content,
                bookData.DescriptionData.DescriptionSource);

            if (description == book.Description)
                return Task.CompletedTask;

            var @event =
                new BookDescriptionChanged(book.Guid, description.BookDescription, description.DescriptionSource);

            book.ApplyChange(@event);

            return Task.CompletedTask;
        }
    }
}