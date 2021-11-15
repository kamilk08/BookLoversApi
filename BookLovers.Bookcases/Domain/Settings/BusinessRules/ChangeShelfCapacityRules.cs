using System.Collections.Generic;
using BookLovers.Base.Domain.Rules;

namespace BookLovers.Bookcases.Domain.Settings.BusinessRules
{
    internal sealed class ChangeShelfCapacityRules : BaseBusinessRule, IBusinessRule
    {
        protected sealed override List<IBusinessRule> FollowingRules { get; } =
            new List<IBusinessRule>();

        public ChangeShelfCapacityRules(
            SettingsManager settingsManager,
            ShelfCapacity shelfCapacity,
            int selectedCapacity)
        {
            FollowingRules.Add(new AggregateMustExist(settingsManager.Guid));
            FollowingRules.Add(new AggregateMustBeActive(settingsManager.AggregateStatus.Value));
            FollowingRules.Add(new ShelfCapacityCannotBeLessThenMinimalValue(shelfCapacity, selectedCapacity));
            FollowingRules.Add(new ShelfCapacityCannotExceedMaximumValue(shelfCapacity, selectedCapacity));
        }

        public bool IsFulfilled() => !AreFollowingRulesBroken();

        public string BrokenRuleMessage => Message;
    }
}