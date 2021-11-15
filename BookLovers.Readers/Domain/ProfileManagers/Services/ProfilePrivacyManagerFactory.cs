using System;
using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;
using BookLovers.Base.Domain.Services;

namespace BookLovers.Readers.Domain.ProfileManagers.Services
{
    public class ProfilePrivacyManagerFactory : IDomainService<ProfilePrivacyManager>
    {
        private readonly List<Func<ProfilePrivacyManager, IBusinessRule>> _rules =
            new List<Func<ProfilePrivacyManager, IBusinessRule>>();

        public ProfilePrivacyManagerFactory()
        {
            _rules.Add(manager => new AggregateMustBeActive(manager.AggregateStatus.Value));
        }

        public ProfilePrivacyManager CreatePrivacyManager(Guid profileGuid)
        {
            var profilePrivacyManager = new ProfilePrivacyManager(Guid.NewGuid(), profileGuid);

            foreach (var currentOption in CurrentPrivacyOptions.CurrentOptions)
                profilePrivacyManager.AddPrivacyOption(currentOption.Key);

            foreach (var rule in _rules)
            {
                if (!rule(profilePrivacyManager).IsFulfilled())
                    throw new BusinessRuleNotMetException(rule(profilePrivacyManager).BrokenRuleMessage);
            }

            return profilePrivacyManager;
        }
    }
}