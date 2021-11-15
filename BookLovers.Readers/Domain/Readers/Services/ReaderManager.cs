using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Events.Readers;

namespace BookLovers.Readers.Domain.Readers.Services
{
    internal class ReaderManager : IAggregateManager<Reader>
    {
        private readonly List<Func<Reader, IBusinessRule>> _rules =
            new List<Func<Reader, IBusinessRule>>();

        public ReaderManager()
        {
            _rules.Add(r => new AggregateMustExist(r.Guid));
            _rules.Add(r => new AggregateMustBeActive(r.AggregateStatus.Value));
        }

        public void Archive(Reader aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new ReaderSuspended(aggregate.Guid, aggregate.Socials.ProfileGuid,
                aggregate.Socials.NotificationWallGuid));
        }
    }
}