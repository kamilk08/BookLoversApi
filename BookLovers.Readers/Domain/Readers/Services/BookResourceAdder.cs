using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.BusinessRules;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Domain.Readers.Services
{
    public class BookResourceAdder : IResourceAdder
    {
        private readonly List<Func<Reader, IAddedResource, IBusinessRule>> _rules =
            new List<Func<Reader, IAddedResource, IBusinessRule>>();

        public AddedResourceType AddedResourceType => AddedResourceType.Book;

        public BookResourceAdder()
        {
            _rules.Add((reader, resource) => new AggregateMustBeActive(reader.AggregateStatus.Value));
            _rules.Add((reader, resource) => new AddedResourceCannotBeDuplicated(reader, resource));
        }

        public void AddResource(Reader reader, IAddedResource addedResource)
        {
            foreach (var rule in _rules)
            {
                if (!rule(reader, addedResource).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(reader, addedResource).BrokenRuleMessage);
            }

            var addedBook = addedResource as AddedBook;

            var @event = ReaderAddedBook
                .Initialize()
                .WithAggregate(reader.Guid)
                .WithBook(addedBook.BookGuid, addedBook.BookId)
                .WithAddedAt(addedBook.AddedAt);

            reader.ApplyChange(@event);
        }
    }
}