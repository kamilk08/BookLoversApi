using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.BusinessRules;

namespace BookLovers.Readers.Domain.Readers.Services
{
    public class ReaderFactory
    {
        private readonly List<Func<Reader, IBusinessRule>> _rules =
            new List<Func<Reader, IBusinessRule>>();

        public ReaderFactory()
        {
            _rules.Add(reader => new AggregateMustBeActive(reader.AggregateStatus.Value));
            _rules.Add(reader => new ReaderMustHaveTimeline(reader));
            _rules.Add(reader => new ReaderMustBeAssociatedWithProfile(reader));
        }

        public Reader Create(
            Guid aggregateGuid,
            ReaderIdentification identification,
            ReaderSocials readerSocials)
        {
            var reader = new Reader(aggregateGuid, identification, readerSocials);

            foreach (var rule in _rules)
            {
                if (!rule(reader).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(reader).BrokenRuleMessage);
            }

            return reader;
        }
    }
}