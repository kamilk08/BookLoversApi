using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Profiles.BusinessRules;

namespace BookLovers.Readers.Domain.Profiles.Services.Factories
{
    public class ProfileFactory
    {
        private readonly List<Func<Profile, IBusinessRule>> _rules =
            new List<Func<Profile, IBusinessRule>>();

        public ProfileFactory()
        {
            _rules.Add(profile => new AggregateMustBeActive(profile.AggregateStatus.Value));
            _rules.Add(profile => new SexTypeMustBeValid(profile.Identity.Sex));
        }

        public Profile CreateProfile(ProfileData data)
        {
            var profile = new Profile(data.ProfileGuid, data.ReaderGuid, data.JoinedAt);

            foreach (var rule in _rules)
            {
                if (!rule(profile).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(profile).BrokenRuleMessage);
            }

            return profile;
        }
    }
}