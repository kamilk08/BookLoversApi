using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.BusinessRules;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Domain.Readers.Services
{
    public class ReviewResourceAdder : IResourceAdder
    {
        private readonly List<Func<Reader, IAddedResource, IBusinessRule>> _rules =
            new List<Func<Reader, IAddedResource, IBusinessRule>>();

        public AddedResourceType AddedResourceType => AddedResourceType.Review;

        public ReviewResourceAdder()
        {
            _rules.Add((reader, resource) => new AggregateMustExist(reader.Guid));
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

            var readerReview = addedResource as ReaderReview;

            var readerAddedReview = ReaderAddedReview
                .Initialize()
                .WithAggregate(reader.Guid)
                .WithBookReview(readerReview.ReviewGuid, readerReview.BookGuid)
                .WithAddedAt(readerReview.AddedAt);

            reader.ApplyChange(readerAddedReview);
        }
    }
}