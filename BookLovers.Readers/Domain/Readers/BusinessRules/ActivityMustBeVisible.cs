using BookLovers.Base.Domain.Rules;
using BookLovers.Readers.Domain.Readers.TimeLineActivities;

namespace BookLovers.Readers.Domain.Readers.BusinessRules
{
    internal class ActivityMustBeVisible : IBusinessRule
    {
        private const string BrokenRuleErrorMessage = "Activity must be visible to hide it.";

        private readonly Activity _activity;

        public ActivityMustBeVisible(Activity activity)
        {
            _activity = activity;
        }

        public bool IsFulfilled() => _activity.ShowToOthers;

        public string BrokenRuleMessage => BrokenRuleErrorMessage;
    }
}