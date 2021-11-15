using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookHashTagsEditor : IBookEditor
    {
        private readonly List<Func<Book, IBusinessRule>> _rules;

        public BookHashTagsEditor()
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

            var list = bookData.BookHashTags.Select(s => new BookHashTag(s)).ToList();

            if (book.HashTags.ToList().SequenceEqual(list))
                return Task.CompletedTask;

            var @event = new BookHashTagsChanged(book.Guid, list.Select(s => s.HashTagContent).ToList());

            book.ApplyChange(@event);

            return Task.CompletedTask;
        }
    }
}