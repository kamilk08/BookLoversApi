using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookLovers.Base.Domain.Rules;
using BookLovers.Publication.Domain.Books.BusinessRules;
using BookLovers.Publication.Events.Book;

namespace BookLovers.Publication.Domain.Books.Services.Editors
{
    public class BookCoverEditor : IBookEditor
    {
        private readonly List<Func<Book, BookData, IBusinessRule>> _rules;

        public BookCoverEditor()
        {
            _rules = new List<Func<Book, BookData, IBusinessRule>>();

            _rules.Add((book, data) => new AggregateMustBeActive(book.AggregateStatus.Value));
            _rules.Add((book, data) => new CoverTypeMustBeValid(data.CoverData.CoverType));
        }

        public Task EditBook(Book book, BookData bookData)
        {
            foreach (var rule in _rules)
            {
                if (!rule(book, bookData).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(book, bookData).BrokenRuleMessage);
            }

            var cover = new Cover(bookData.CoverData.CoverType, bookData.CoverData.CoverSource);
            if (cover == book.Cover)
                return Task.CompletedTask;

            var @event = CoverChanged.Initialize()
                .WithAggregate(book.Guid)
                .WithCover(cover.CoverType.Value, cover.CoverSource);

            book.ApplyChange(@event);

            return Task.CompletedTask;
        }
    }
}