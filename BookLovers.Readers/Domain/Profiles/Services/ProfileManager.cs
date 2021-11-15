using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;
using BookLovers.Readers.Events.Profile;

namespace BookLovers.Readers.Domain.Profiles.Services
{
    internal class ProfileManager : IAggregateManager<Profile>
    {
        private readonly List<Func<Profile, IBusinessRule>> _rules =
            new List<Func<Profile, IBusinessRule>>();

        public ProfileManager()
        {
            _rules.Add(profile => new AggregateMustBeActive(profile.AggregateStatus.Value));
        }

        public void Archive(Profile aggregate)
        {
            foreach (var rule in _rules)
            {
                if (!rule(aggregate).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(aggregate).BrokenRuleMessage);
            }

            aggregate.ApplyChange(new ProfileArchived(aggregate.Guid));
        }
    }
}